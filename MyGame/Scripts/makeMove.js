

function SetupMove(model, color) {
    var $Boxes,
        stepHub,
        $figures

    var validBoxes = [];

    stepHub = $.connection.stepHub,
    $figures = $(".figure_" + color),
    $Boxes = $(".blackSquares");

    stepHub.client.reciveJoinSignal = function (joinModel) {
        model.OpponentName = joinModel.myName;
        if (model.isMyTurn) {
            $("#TurnStatusParag").html("Now it's your turn!");
            $figures.draggable('enable');
        }
        else {
            $("#TurnStatusParag").html("Waiting for opponent's turn...");
        }
    }
    stepHub.client.changePosition = function (step) {
        ChangePos(step, $figures);      
    }

    $.connection.hub.start().done(function () {
        var $validBoxe_left;
        var $validBoxe_right;

        $figures.draggable({
            containment: $("#checkersTable"),
            stop: function () {
                validBoxes = StopDrag(validBoxes, $(this));
            },
            start: function () {
                StartDrag(validBoxes, $(this));
            }

        });

        $Boxes.droppable({
            drop: function (event, ui) {
                Drop(ui, $(this), color, $figures, model, stepHub);
            }
        })

        if (!model.isMyTurn) {
            $("#TurnStatusParag").html("Waiting for opponent's turn...");
            $figures.draggable('disable');
        }
        else {
            $("#TurnStatusParag").html("Now it's your turn!");
        }

        if (model.OpponentName == null) {
            $("#TurnStatusParag").html("Waiting for second player...");
            $figures.draggable('disable');
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

function Drop(ui, box, color, figures, model, stepHub) {
    var col = box.css("background-color");
    if (col == "rgb(0, 128, 0)") {
        var x = box.data("xcoord"),
            y = box.data("ycoord");
        if (color == "Black") {
            x = 11 - x;
            y = 11 - y;
        }
        $.post("/Game/ChangeFigurePos", { model: { GameId: model.GameId, FigureId: ui.draggable.data("fig-id"), NewYPos: y, NewXPos: x } },
            MakeAppend(box, ui, figures, stepHub, model),
            "json");
    }
}

function StopDrag(validBoxes, current) {
    validBoxes.forEach(function (elem) {
        elem.css("background-color", "#3B363A");
    });
    validBoxes = [];
    current.css("z-index", "10");
    current.css({ left: 0, top: 0 });
    return validBoxes;
}

function StartDrag(validBoxes, currentFigure) {
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

function MakeAppend(box, ui, figures, stepHub, model) {
    box.append(ui.draggable);
    var stepModel = {
        newY: box.data("ycoord"),
        newX: box.data("xcoord"),
        figureId: ui.draggable.data("fig-id"),
        gameId: model.GameId,
        receiverName: model.OpponentName,
    }
    ui.draggable.css("z-index", "10");
    ui.draggable.css({ left: 0, top: 0 });
    $("#TurnStatusParag").html("Waiting for opponent's turn...");
    figures.draggable('disable');
    stepHub.server.updateField(stepModel);
}

function ChangePos(step, figures) {
    var newSquareX = 11 - step.newX;
    var newSquareY = 11 - step.newY;
    var $newParent = $(".blackSquares[data-xcoord='" + newSquareX + "'][data-ycoord='" + newSquareY + "']");
    var $currentFigure = $("img[data-fig-id='" + step.figureId + "']");
    var $oldParent = $currentFigure.parent();

    var xOld = $oldParent.offset().left;
    var xNew = $newParent.offset().left;

    var yOld = $oldParent.offset().top;
    var yNew = $newParent.offset().top;

    var $figureClone = $currentFigure.clone();
    $currentFigure.hide();
    $newParent.append($currentFigure);
    $oldParent.append($figureClone);
    $figureClone.animate({
        left: xNew - xOld,
        top: yNew - yOld
    }, "slow", function () {
        $oldParent.empty();
        $currentFigure.show();
        //getPotencialKillers($figures, $(".blackSquares"), $newParent, color);
    });
    figures.draggable('enable');
    $("#TurnStatusParag").html("Now it's your turn!");
}