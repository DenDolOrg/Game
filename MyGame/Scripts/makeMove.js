function SetupMove(model, color) {
    var stepHub = $.connection.stepHub;
    var boxes = $(".blackSquares");  
    var figures =  $(".figure_" + color);
    var isRegularMovement = true;      
    var validBoxes = [];
    var killModels; 

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

        if (step.figureDelId != null) {
            var figToDelate = $(".figure_" + color + "[data-fig-id=" + step.figureDelId + "]");
            figures = figures.filter(function (index) {
                return $(this).data("fig-id") != figToDelate.data("fig-id");
            });
            figToDelate.parent().empty();

        }
        killModels = ChangePos(step, figures, boxes, color);

        if (killModels != null && killModels.length != 0) {
            isRegularMovement = false;
            PrepareToEat(figures, killModels);
        }
        else {
            isRegularMovement = true;
        }

    }

    $.connection.hub.start().done(function () {
        figures.draggable({
            containment: $("#checkersTable"),
            stop: function () {
                if (isRegularMovement) {
                    validBoxes = StopRegularDrag(validBoxes, $(this));
                }
                else {
                    validBoxes = StopEatDrag(validBoxes, $(this));
                }
            },
            start: function () {
                if (isRegularMovement) {
                    StartRegularDrag(validBoxes, $(this));
                }
                else {
                    var current = $(this);
                    var currentModel = killModels.filter(o => {
                        var $killer = o.killer;
                        return $killer.data("fig-id") == current.data("fig-id")
                    });
                    validBoxes = currentModel[0].freeSpaces;
                    StartEatDrag(validBoxes, $(this))
                }
                
            }

        });

        boxes.droppable({
            drop: function (event, ui) {
                if (isRegularMovement) {
                    RegularDrop(ui, $(this), color, figures, model, stepHub, null);
                }
                else {
                    EatDrop(ui, $(this), color, figures, model, stepHub, killModels, isRegularMovement);
                }
            }
        });

        if (!model.isMyTurn) {
            $("#TurnStatusParag").html("Waiting for opponent's turn...");
            figures.draggable('disable');
        }
        else {
            killModels = getPotencialKillers(figures, boxes, color);

            if (killModels != null && killModels.length != 0) {
                isRegularMovement = false;
                PrepareToEat(figures, killModels);
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
            stepHub.server.joinSignal(joinModel)
        }

    });

};

function PrepareToEat(figures, killModels) {
    figures.draggable('disable');
    killModels.forEach(function (model) {
        var parent = model.killer.parent();
        parent.addClass("canEat");
        model.killer.draggable('enable');

    });
}

function RegularDrop(ui, box, color, figures, model, stepHub, figureIdToDelete) {
    var col = box.css("background-color");
    if (col == "rgb(0, 128, 0)") {
        var x = box.data("xcoord"),
            y = box.data("ycoord"),
            delta = 0;
        if (color == "Black") {
            delta = 11;
        }
        var stepModel = {
            newY: y,
            newX: x,
            figureId: ui.draggable.data("fig-id"),
            gameId: model.GameId,
            receiverName: model.OpponentName,
            figureDelId: figureIdToDelete
        };
        SendDataToServer(stepModel, box, ui, stepHub, delta);
        
        figures.draggable('disable');
    }
}

function EatDrop(ui, box, color, figures, model, stepHub, killModels) {
    var col = box.css("background-color");
    if (col == "rgb(0, 128, 0)") {
        var currentKillModel = killModels.filter(o => {
            var $killer = o.killer;
            return $killer.data("fig-id") == ui.draggable.data("fig-id")
        });

        var victimes = currentKillModel[0].victimes;
        var fields = currentKillModel[0].freeSpaces;
        var victim;
        fields.forEach(function (elem, index, array) {
            if ((elem.data("xcoord") == box.data("xcoord")) &&
                (elem.data("ycoord") == box.data("ycoord"))) {
                victim = victimes[index];
            }
        });
        var figureId = victim.children().data("fig-id");

        RegularDrop(ui, box, color, figures, model, stepHub, figureId)
        victim.empty();
    }
}

function StopRegularDrag(validBoxes, current) {
    validBoxes.forEach(function (elem) {
        elem.css("background-color", "#3B363A");
    });
    validBoxes = [];
    current.css("z-index", "10");
    current.css({ left: 0, top: 0 });
    return validBoxes;
}

function StopEatDrag(validBoxes, current) {
    validBoxes = StopRegularDrag(validBoxes, current);

    return validBoxes;
}

function StartRegularDrag(validBoxes, currentFigure) {
    currentFigure.css("z-index", "15");
    var validY = currentFigure.parent().data("ycoord") - 1;
    var validX_r = currentFigure.parent().data("xcoord") + 1;
    var validX_l = validX_r - 2;
    var $left = $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_l + "']");
    var $right = $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_r + "']");
    validBoxes.push($left);
    validBoxes.push($right);

    validBoxes.forEach(function (elem) {
        elem.css("background-color", "green");
    });
}

function StartEatDrag(validBoxes, currentFigure) {
    currentFigure.css("z-index", "15");
    validBoxes.forEach(function (elem) {
        elem.css("background-color", "green");
    });
}

function MakeAppend(box, ui, stepHub, stepModel) {
    box.append(ui.draggable);
    ui.draggable.css("z-index", "10");
    ui.draggable.css({ left: 0, top: 0 });
    $("#TurnStatusParag").html("Waiting for opponent's turn...");
    $(".canEat").removeClass("canEat");  
    stepHub.server.updateField(stepModel);
}

function RemoveFigure(figureId, stepHub, model) {
    var deleteModel = {
        receiverName: model.OpponentName,
        figureId: figureId
    }
    stepHub.server.deletetFigure(deleteModel);
}

function SendDataToServer(stepModel, box, ui, stepHub, delta) {
        $.post("/Game/ChangeField", {
            model: {
                GameId: stepModel.gameId,
                ReceiverName: stepModel.receiverName,
                FigureId: stepModel.figureId,
                FigureIdToDelete: stepModel.figureDelId,
                NewXPos: Math.abs(stepModel.newX - delta),
                NewYPos: Math.abs(stepModel.newY - delta)
            }
        },
            MakeAppend(box, ui, stepHub, stepModel),
        "json");
}

function ChangePos(step, figures, boxes, color) {
    var newSquareX = 11 - step.newX;
    var newSquareY = 11 - step.newY;
    var newParent = $(".blackSquares[data-xcoord='" + newSquareX + "'][data-ycoord='" + newSquareY + "']");
    var currentFigure = $("img[data-fig-id='" + step.figureId + "']");
    var oldParent = currentFigure.parent();

    var xOld = oldParent.offset().left;
    var xNew = newParent.offset().left;

    var yOld = oldParent.offset().top;
    var yNew = newParent.offset().top;

    var figureClone = currentFigure.clone();
    currentFigure.hide();
    newParent.append(currentFigure);
    killModels = getPotencialKillers(figures, boxes, color);
    oldParent.append(figureClone);
    var killModels;
    figureClone.animate({
        left: xNew - xOld,
        top: yNew - yOld
    }, "slow", function () {
        oldParent.empty();
        currentFigure.show();  
        });

        $("#TurnStatusParag").html("Now it's your turn!");
        return killModels;   
}