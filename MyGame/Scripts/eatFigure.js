function getPotencialKillers(color) {

    var opponentColor = "White";
    var killModels = [];

    if (color == "White") {
        opponentColor = "Black";
    }
    var figures = $(".figure_" + color).not(".toDelete");

    figures.each(function () {
        var current = $(this);
        var model = GetKillerModel(current, current.parent().data("xcoord"), current.parent().data("ycoord"), opponentColor);
        if (model != null) {
            killModels.push(model);
        }
    });
    return killModels;
}

function GetKillerModel(currentFig, x, y, opponentColor) {
    var killModel = null;
    var freeBoxes = $(".blackSquares").not(":has(img:not(.toDelete))");
    var oppFields = $(".figure_" + opponentColor).parent();
    var currentFig_X = x
    var currentFig_Y = y

    var angles = oppFields.filter(function (index) {
        var elemX = $(this).data("xcoord");
        var elemY = $(this).data("ycoord");
        var res =
            (((elemX == currentFig_X + 1) && (elemY == currentFig_Y + 1)) ||
            ((elemX == currentFig_X - 1) && (elemY == currentFig_Y - 1)) ||
            ((elemX == currentFig_X + 1) && (elemY == currentFig_Y - 1)) ||
            ((elemX == currentFig_X - 1) && (elemY == currentFig_Y + 1)));

        return res;
    });

    angles.each(function () {
        var angleFig = $(this);
        var angleFig_X = $(this).data("xcoord");
        var angleFig_Y = $(this).data("ycoord");

        var tempRealSpaces = freeBoxes.filter(function (index) {
            var elemX = $(this).data("xcoord");
            var elemY = $(this).data("ycoord");
            return ((elemX == 2 * angleFig_X - currentFig_X) && (elemY == 2 * angleFig_Y - currentFig_Y))
        });
        if (tempRealSpaces.length != 0) {
            angleFig.addClass("victim");
            tempRealSpaces.addClass("freeBox");
        }     
    })
    var victimes = $(".victim");
    var realFreeSpaces = $(".freeBox");

    if (realFreeSpaces.length != 0) {
        killModel = new KillModel(currentFig, realFreeSpaces, victimes, null);
        victimes.removeClass("victim");
        realFreeSpaces.removeClass("freeBox");
    }
    return killModel;
}

function KillModel(killer, freeSpaces, victimes) {
    this.killer = killer;
    this.freeSpaces = freeSpaces;
    this.victimes = victimes;
}