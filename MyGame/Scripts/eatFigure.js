function getPotencialKillers(figures, boxes, newParent, color) {

    var opponentColor = "White";
    var killModels = [];

    if (color == "White") {
        opponentColor = "Black";
    }


    figures.each(function () {
        var currentFig = $(this);
        var currentFig_X = currentFig.parent().data("xcoord");
        var currentFig_Y = currentFig.parent().data("ycoord");

        var angles = boxes.filter(function (index) {
            var elemX = $(this).data("xcoord");
            var elemY = $(this).data("ycoord");
            return (((elemX == currentFig_X + 1) && (elemY == currentFig_Y + 1)) ||
                ((elemX == currentFig_X - 1) && (elemY == currentFig_Y - 1)) ||
                ((elemX == currentFig_X + 1) && (elemY == currentFig_Y - 1)) ||
                ((elemX == currentFig_X - 1) && (elemY == currentFig_Y + 1)))
        });

        var anglesWithOpponentFigures = angles.has(".figure_" + opponentColor);

        var realFreeSpaces = [];

        anglesWithOpponentFigures.each(function () {
            var angleFig = $(this);
            var angleFig_X = $(this).data("xcoord");
            var angleFig_Y = $(this).data("ycoord");


            var potencialFreeSpace = boxes.filter(function (index) {
                var elemX = $(this).data("xcoord");
                var elemY = $(this).data("ycoord");
                return ((elemX == 2 * angleFig_X - currentFig_X) && (elemY == 2 * angleFig_Y - currentFig_Y))
            });

            if (potencialFreeSpace.length != 0) {
                var real = potencialFreeSpace.not(":has(img)");
                if (real.length != 0) {
                    realFreeSpaces.push(real);
                }
            }
        });

        if (realFreeSpaces.length != 0) {
            killModels.push(new KillModel(currentFig, realFreeSpaces));
        }
    });

    if (killModels.length == 0) {
        figures.draggable('enable');
    }
    else {
        figures.draggable('disable');
        killModels.forEach(function (model) {
            model.killer.draggable('enable');
            model.killer.parent().css("background-color", "#FFA900");
        });
    }
}

function KillModel(killer, victims) {
    this.killer = killer;
    this.victims = victims;
}

function EatMove(model) {

    figures.draggable({
        containment: $("#checkersTable"),
        stop: function () {
            var current = $(this);
            model.forEach(function (m) {
                m.killer.css("background-color", "FFA900");
            });
            current.css("z-index", "10");
            current.css({ left: 0, top: 0 });
        },
        start: function () {
            var currentFigure = $(this);
            currentFigure.css("z-index", "15");
            boxes.forEach(function (box) {
                box.css("background-color", "green");
            });

        }
    });

    model.victims.droppable({
        drop: function (event, ui) {
            var box = $(this);
            var col = box.css("background-color");
            if (col == "rgb(0, 128, 0)") {
                var x = box.data("xcoord"),
                    y = box.data("ycoord");
                if (color == "Black") {
                    x = 11 - x;
                    y = 11 - y;
                }
                $.post("/Game/ChangeFigurePos", { model: { GameId: model.GameId, FigureId: ui.draggable.data("fig-id"), NewYPos: y, NewXPos: x } },
                    MakeAppend(box, ui, $figures, stepHub, model),
                    "json");
            }
        }
    })
}