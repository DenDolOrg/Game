function getPotencialKillers(figures, boxes, color) {

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
        var victimes = [];

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
                    victimes.push(angleFig);
                }
            }
        });

        if (realFreeSpaces.length != 0) {
            killModels.push(new KillModel(currentFig, realFreeSpaces, victimes));
        }
    });

    return killModels;
}

function KillModel(killer, freeSpaces, victimes) {
    this.killer = killer;
    this.freeSpaces = freeSpaces;
    this.victimes = victimes;
}