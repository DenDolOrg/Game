function SetupMove(model, color) {
    var stepHub = $.connection.stepHub;
    var boxes = $(".blackSquares");  
    var figures =  $(".figure_" + color);
    var isRegularMovement = true;      
    var idsToDel = "";
    var coordsToMove = "";
    var coordsToMove = "";
    var killModels; 
    var superFigureStatus = 0;
    stepHub.client.reciveEndOfGame = function () {
        alert("You win.");
    }

    stepHub.client.reciveJoinSignal = function (joinModel) {
        model.OpponentName = joinModel.myName;
        if (model.isMyTurn) {
            $("#TurnStatusParag").html("Now it's your turn!");
            figures.draggable('enable');
        }
        else {
            $("#TurnStatusParag").html("Waiting for opponent's turn...");
        }
    }

    stepHub.client.changeField = function (step) {
        figures.draggable('enable');
        idsToDel = "";
        coordsToMove = "";
        var figuserToDelate = [];
        if (step.figureDelId != null) {        
            var figIdsToDelete = step.figureDelId.split(",");
            figIdsToDelete.forEach(function (id) {
                figuserToDelate.push($(".figure_" + color + "[data-fig-id=" + id + "]"));
            });
            figuserToDelate.forEach(function (fig) {
                fig.addClass("toDelete");
            });
            figures = figures.not(".toDelete");
            
        }
        killModels = ChangePos(step, color);
        if (figures.length != 0) {

            if (killModels != null && killModels.length != 0) {
                isRegularMovement = false;
                figures.draggable('disable');
                PrepareToEat(killModels);
            }
            else {
                isRegularMovement = true;
            }
        }
        else {
            GameLose(stepHub, model.OpponentName);
        }
    }

    $.connection.hub.start().done(function () {
        figures.draggable({
            containment: $("#checkersTable"),
            stop: function () {
                var currentFigure = $(this);
                currentFigure.css("z-index", "10");
                currentFigure.css({ left: 0, top: 0 });
                StopDrag();
            },
            start: function () {
                var currentFigure = $(this);
                if (currentFigure.is(".superFig")) {
                    superFigureStatus = 1;
                }
                else {
                    superFigureStatus = 0;
                }
                if (isRegularMovement) {
                    currentFigure.css("z-index", "15");
                    if (superFigureStatus == 1) {
                        SetBoxesForSuperFigureDrag(currentFigure);
                    }
                    else {
                        SetBoxesForRegularDrag(currentFigure);
                    }
                    StartDrag();
                }
                else {
                    var currentFigure = $(this);
                    var currentModel = killModels.filter(o => {
                        var killer = o.killer;
                        return killer.data("fig-id") == currentFigure.data("fig-id")
                    });
                    currentModel[0].freeSpaces.addClass("validBox");
                    StartDrag();
                }              
            }

        });

        boxes.droppable({
            drop: function (event, ui) {
                var droppedFigure = ui.draggable;
                if (isRegularMovement) {
                    RegularDrop(droppedFigure, $(this), color, figures, model, stepHub, null, null, superFigureStatus);
                }
                else {
                    var data = EatDrop(droppedFigure, $(this), color, figures, model, stepHub, killModels, idsToDel, coordsToMove, superFigureStatus);
                    if (data != null) {            
                        idsToDel += data.idsToDel + ",";
                        coordsToMove += data.coordsToMove + ";";

                        if (data.innerModel != null) {
                            var tmp = [];
                            tmp.push(data.innerModel);
                            killModels = tmp;
                        }
                    }
                }
            }
        });

        if (!model.isMyTurn) {
            $("#TurnStatusParag").html("Waiting for opponent's turn...");
            figures.draggable('disable');
        }
        else {
            killModels = getPotencialKillers(color);

            if (killModels != null && killModels.length != 0) {
                isRegularMovement = false;
                figures.draggable('disable');
                PrepareToEat(killModels);
            }
            $("#TurnStatusParag").html("Now it's your turn!");
        }

        if (model.OpponentName == null) {
            $("#TurnStatusParag").html("Waiting for second player...");
            figures.draggable('disable');
        };

        if (model.OpponentName != null) {
            var joinModel = {
                receiverName: model.OpponentName,
                myName: model.MyName
            };
            stepHub.server.joinSignal(joinModel);
        }
    });
};

function PrepareToEat(killModels) {
    
    killModels.forEach(function (model) {
        var parent = model.killer.parent();
        parent.addClass("canEat");
        model.killer.draggable('enable');

    });
}

function ContinueToEat(killModel) {
    var parent = killModel.killer.parent();
    parent.addClass("canEat");
    killModel.killer.draggable('enable');
}

function RegularDrop(droppedFigure, box, color, figures, model, stepHub, figureIdsToDelete, movesToDo, superFigureStatus) {
    var col = box.css("background-color");
    if (col == "rgb(0, 128, 0)") {
        var x = box.data("xcoord"),
            y = box.data("ycoord"),
            delta = 0;
        if (color == "Black") {
            delta = 11;
        }
        if (movesToDo == null) {
            movesToDo = (x) + "," + y;
        }
        if (y == 1) {
            superFigureStatus = 1;
            droppedFigure.addClass("superFig");
            if (color == "Black") {
                $(droppedFigure).attr("src", "../../Images/damkaB.png");
            }
            else {
                $(droppedFigure).attr("src", "../../Images/damkaW.png");
            }  
        }
        var stepModel = {
            figureId: droppedFigure.data("fig-id"),
            gameId: model.GameId,
            receiverName: model.OpponentName,
            figureDelId: figureIdsToDelete,
            coordsToMove: movesToDo,
            superFigStatus: superFigureStatus
        };
        SendDataToServer(stepModel, stepHub, delta);
        MakeAppend(box, droppedFigure);
       
        figures.draggable('disable');
    }
}

function EatDrop(droppedFigure, box, color, figures, model, stepHub, killModels, figureIdsToDelete, movesToDo, superFigureStatus) {
    var col = box.css("background-color");
    var data = null;
    if (col == "rgb(0, 128, 0)") {
        var boxX = box.data("xcoord");
        var boxY = box.data("ycoord");
        if (boxY == 1) {
            superFigureStatus = 1;
            droppedFigure.addClass("superFig");
            if (color == "Black") {
                $(droppedFigure).attr("src", "../../Images/damkaB.png");
            }
            else {
                $(droppedFigure).attr("src", "../../Images/damkaW.png");
            }          
        }
        var currentKillModel = killModels.filter(o => {
            var $killer = o.killer;
            return $killer.data("fig-id") == droppedFigure.data("fig-id")
        });

        var victimes = currentKillModel[0].victimes;
        var victim = victimes[0];
        var minDist = 100;
        victimes.each(function (index) {
            var dist = Math.sqrt(Math.pow((boxX - $(this).data("xcoord")), 2) + Math.pow((boxY - $(this).data("ycoord")), 2));
            if (dist < minDist) {
                minDist = dist;
                victim = $(this);
            }
        });
        victim.children().addClass("toDelete");
        var coords = boxX + "," + boxY;
        data = new SendDataModel(null, victim.children().data("fig-id"), coords, superFigureStatus);

        var opponentColor = "White";
        if (color == "White") {
            opponentColor = "Black";
        }

        var innerModel = GetKillerModel(droppedFigure, boxX, boxY, color)
        if (innerModel != null) {
            data.innerModel = innerModel;
            MakeAppend(box, droppedFigure);
            figures.draggable('disable');
            ContinueToEat(innerModel);
            
        }
        else {
            $(".toDelete").remove();
            figureIdsToDelete += data.idsToDel;
            movesToDo += data.coordsToMove;
            RegularDrop(droppedFigure, box, color, figures, model, stepHub, figureIdsToDelete, movesToDo, superFigureStatus);
        }
    }
    return data;
}

function StopDrag() {
    validBoxes = $(".validBox");
    
    validBoxes.each(function (index) {
        $(this).css("background-color", "#3B363A");
    });
    validBoxes.removeClass("validBox");
}

function StartDrag() {
    
    validBoxes = $(".validBox");
    validBoxes.each(function (index) {
        $(this).css("background-color", "green");
    });
}

function MakeAppend(box, droppedFigure) {
    box.append(droppedFigure);
    droppedFigure.css("z-index", "10");
    droppedFigure.css({ left: 0, top: 0 });
    $("#TurnStatusParag").html("Waiting for opponent's turn...");
    $(".canEat").removeClass("canEat");  
    
}

function SendDataToServer(stepModel, stepHub, delta) {
    var newLastPair = PrepareCoordForServer(stepModel, delta)
    var model = {
        GameId: stepModel.gameId,
        ReceiverName: stepModel.receiverName,
        FigureId: stepModel.figureId,
        FigureIdsToDelete: stepModel.figureDelId,
        CoordsToMove: newLastPair,
        NewSuperFigureStatus: stepModel.superFigStatus
    }
    $.ajax({
        url: "/Game/ChangeField",
        type: "POST",
        data: JSON.stringify({ data: model}),
        contentType: "application/json; charset=utf-8",
        success: function () {
            stepHub.server.updateField(stepModel);
        }      
    });
}

function PrepareCoordForServer(stepModel, delta) {
    var coordPairs = stepModel.coordsToMove.split(";");
    var lastpair = coordPairs[coordPairs.length - 1].split(",");
    var lastX = Math.abs(parseInt(lastpair[0], 10) - delta);
    var lastY = Math.abs(parseInt(lastpair[1], 10) - delta);
    var newLastPair = lastX + "," + lastY;
    return newLastPair;
}

function ChangePos(step, color) {
    var coordPairs = step.coordsToMove.split(";");

    var coordModels = [];
    coordPairs.forEach(function (pair) {    
        var XandY = pair.split(",");
        coordModels.push(new CoordModel(11 - parseInt(XandY[0], 10), 11 - parseInt(XandY[1]), null));
    });

    coordModels.forEach(function (model) {
        model.newParent = $(".blackSquares[data-xcoord='" + model.x + "'][data-ycoord='" + model.y + "']")
    });

    var currentFigure = $("img[data-fig-id='" + step.figureId + "']");

    var killerModels = AnimateMovement(0, coordModels, currentFigure, color);

    return killerModels;
    
}

function AnimateMovement(movementIndex, coordModels, currentFigure, color) {
    var killModels = null;

    var newParent = coordModels[movementIndex].newParent;
    var oldParent = currentFigure.parent();

    var xOld = oldParent.offset().left;
    var xNew = newParent.offset().left;

    var yOld = oldParent.offset().top;
    var yNew = newParent.offset().top;

    var figureClone = currentFigure.clone();
    currentFigure.hide();

    if (movementIndex == 0) {
        coordModels[coordModels.length - 1].newParent.append(currentFigure);
        killModels = getPotencialKillers(color);
        coordModels[coordModels.length - 1].newParent.empty();
    }
    newParent.append(currentFigure);
    oldParent.append(figureClone);
    figureClone.animate({
        left: xNew - xOld,
        top: yNew - yOld
    }, "slow", function () {
        oldParent.empty();
        if (coordModels[movementIndex].y == 10) {
            if (color == "Black") {
                currentFigure.attr("src", "../../Images/damkaW.png");
            }
            else {
                currentFigure.attr("src", "../../Images/damkaB.png");
            }         
        }
        currentFigure.show();
        if (movementIndex < coordModels.length - 1) {
            AnimateMovement(movementIndex + 1, coordModels, currentFigure, color);
        }
        else {
            $(".toDelete").remove();
        }
    });
    $("#TurnStatusParag").html("Now it's your turn!");

    return killModels;   
}

function SetBoxesForRegularDrag(currentFigure) {
    var validY = currentFigure.parent().data("ycoord") - 1;
    var validX_r = currentFigure.parent().data("xcoord") + 1;
    var validX_l = validX_r - 2;
    $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_l + "']").addClass("validBox");
    $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_r + "']").addClass("validBox");
}

function SetBoxesForSuperFigureDrag(currentFigure) {
    var current_x = currentFigure.parent().data("xcoord");
    var current_y = currentFigure.parent().data("ycoord");

    SetSuperFigGragDiagonal(current_x, current_y, -1, 1);
    SetSuperFigGragDiagonal(current_x, current_y, -1, -1);
    SetSuperFigGragDiagonal(current_x, current_y, 1, -1);
    SetSuperFigGragDiagonal(current_x, current_y, 1, 1);
}

function SetSuperFigGragDiagonal(current_x, current_y, delta_x, delta_y) {
    var i = 1;
    while (true) {
        var field_x = current_x + i * delta_x;
        var field_y = current_y + i * delta_y;
        if (field_x > 0 && field_x < 11 && field_y > 0 && field_y < 11) {
            var tempFree = $(".blackSquares:not(:has(img))[data-ycoord ='" + field_y + "'][data-xcoord ='" + field_x + "']");
            if (tempFree.is(":not(:has(img))")) {
                tempFree.addClass("validBox");
            }
            else {
                break;
            }
        }
        else {
            break;
        }
        i++;
    }
}

function GameLose(stepHub, name) {
    var endGameModel = {
        receiverName: name
    }
    stepHub.server.endGame(endGameModel);

    alert("You lose.");
}

function SendDataModel(innerModel, idsToDel, coordsToMove, superFigureStatus) {
    this.innerModel = innerModel;
    this.idsToDel = idsToDel;
    this.coordsToMove = coordsToMove;
    this.superFigureStatus = superFigureStatus;
}

function CoordModel(x, y, parent) {
    this.x = x;
    this.y = y;
    this.newParent = parent;
}