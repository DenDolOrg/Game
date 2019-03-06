function getPotencialKillers(color) {
    var killModels = [];

    var figures = $(".figure_" + color).not(".toDelete");

    figures.each(function () {
        var current = $(this);
        var model = GetKillerModel(current, current.parent().data("xcoord"), current.parent().data("ycoord"), color);
        if (model != null) {
            killModels.push(model);
        }
    });
    return killModels;
}

function GetKillerModel(currentFig, x, y, color) {
    var killModel = null;
    var freeBoxes = $(".blackSquares").not(":has(img:not(.toDelete))");
    var currentFig_X = x
    var currentFig_Y = y

    if (currentFig.is(".superFig")) {
        killModel = GetModelForSuperFigure(currentFig, currentFig_X, currentFig_Y, freeBoxes, color, false);
    }
    else {
        killModel = GetModelForNotSuperFigure(currentFig, currentFig_X, currentFig_Y, freeBoxes, color);
    }
    return killModel;
}

function GetModelForNotSuperFigure(currentFig, currentFig_X, currentFig_Y, freeBoxes, color) {
    var angles = [];
    var killModel = null;
    angles = GetAngles(currentFig_X, currentFig_Y, 1, angles, color);
    angles = angles.filter(function (elem) {
        return elem != null;
    });
    var $angles = $(angles).not(":has(.figure_" + color + ")"); 
        $angles.each(function () {
        var angleFig = $(this);
        var angleFig_X = $(this).data("xcoord");
        var angleFig_Y = $(this).data("ycoord");
        var x = 2 * angleFig_X - currentFig_X,
            y = 2 * angleFig_Y - currentFig_Y;
        var tempRealSpaces = $(freeBoxes).filter("[data-xcoord='" + x + "'][data-ycoord='" + y + "']");
        if (tempRealSpaces.length != 0) {
            angleFig.addClass("victim");
            tempRealSpaces.addClass("freeBox");
        }
    })
    var victimes = $(".victim");
    var realFreeSpaces = $(".freeBox");

    if (realFreeSpaces.length != 0) {
        killModel = new KillModel(currentFig, realFreeSpaces, victimes);
        victimes.removeClass("victim");
        realFreeSpaces.removeClass("freeBox");
    }
    return killModel;
}

function GetModelForSuperFigure(currentFig, currentFig_X, currentFig_Y, freeBoxes, color, isInnerCheck) {
    var superAngles = [];
    var killModel = null;
    for (var i = 1; i < 10; i++) {
        superAngles = GetAngles(currentFig_X, currentFig_Y, i, superAngles, color);
    }
    superAngles = superAngles.filter(function (elem) {
        return elem != null;
    });
    var $superAngles = $(superAngles);
    var $filteredSuperAngles = $superAngles.not(":has(.canDelete)").not(":has(.toDelete)").not(":has(.figure_" + color + ")");
    var innerResult = false;
    $filteredSuperAngles.each(function (index) {
        var angleFig = $(this);
        var angleFig_X = $(this).data("xcoord");
        var angleFig_Y = $(this).data("ycoord");
        if (!angleFig.children().is(".canDelete") && !isInnerCheck) {
            angleFig.children().addClass("canDelete");
        }
        if (isInnerCheck) {
            innerResult = SetSpaceFoSuperFigure(angleFig, currentFig_X, currentFig_Y, angleFig_X, angleFig_Y, freeBoxes, color, isInnerCheck)
            if (innerResult) {
                return false;     
            }             
        }
        else {
            SetSpaceFoSuperFigure(angleFig, currentFig_X, currentFig_Y, angleFig_X, angleFig_Y, freeBoxes, color, isInnerCheck);
            
        }        
        
    });  
    if (isInnerCheck) {
        return innerResult;
    }
    $(".canDelete").removeClass("canDelete");
    var victimes = $(".victim");
    var realFreeSpaces = $(".freeBox");

    if (realFreeSpaces.length != 0) {
        killModel = new KillModel(currentFig, realFreeSpaces, victimes);
        victimes.removeClass("victim");
        realFreeSpaces.removeClass("freeBox");
    }
    return killModel;
    }

function SetSpaceFoSuperFigure(angleFig, currentFig_X, currentFig_Y, angleFig_X, angleFig_Y, freeBoxes, color, isInnerCheck) {
    var delta_x = angleFig_X - currentFig_X,
        delta_y = angleFig_Y - currentFig_Y; 
    var realFreeSpaces = [];
    var realKillFreeSpaces = [];

    var field_x, field_y,
        i = 1;
    while (true) {       
        field_x = currentFig_X + (delta_x + i * delta_x / (Math.abs(delta_x)));
        field_y = currentFig_Y + (delta_y + i * delta_y / (Math.abs(delta_y)));
        if (field_x > 0 && field_x < 11 && field_y > 0 && field_y < 11) {
            var tempRealSpaces = freeBoxes.filter("[data-xcoord='" + field_x + "'][data-ycoord='" + field_y + "']");
            if (tempRealSpaces.length != 0) {
                if (isInnerCheck) {
                    return true;
                }
                else {
                    realFreeSpaces.push(tempRealSpaces[0]);
                    if (!angleFig.is(".victim")) {
                        angleFig.addClass("victim");
                    }
                }
            }
            else {
                if (isInnerCheck) {
                    return false;
                }
                break;
            }
        }
        else {
            if (isInnerCheck) {
                return false;
            }
            break;
        }
        i++;
    }
    realFreeSpaces.forEach(function (elem) {
        if (GetModelForSuperFigure(null, $(elem).data("xcoord"), $(elem).data("ycoord"), freeBoxes, color, true)) {
            realKillFreeSpaces.push(elem);
        }
    });
    if (realKillFreeSpaces.length == 0) {
        $(realFreeSpaces).addClass("freeBox");
    }
    else {
        $(realKillFreeSpaces).addClass("freeBox");
    }

}

function GetAngles(currentFig_X, currentFig_Y, dist, angles) {
    var sqr, x, y;
    for (var i = 0; i < 4; i++) {
        if (angles[i] != null) {
            continue;
        }
        x = currentFig_X,
        y = currentFig_Y;
        switch (i) {
            case 0:
                x -= dist;
                y += dist;
                break;
            case 1:
                x -= dist;
                y -= dist;
                break;
            case 2:
                x += dist;
                y -= dist;
                break;
            case 3:
                x += dist;
                y += dist;
                break;
        }
        if (x > 10 || x < 1 || y > 10 || y < 1) {
            continue;
        }
        sqr = $(".blackSquares[data-xcoord='" + x + "'][data-ycoord='" + y + "']")
        if (sqr.length != 0 && sqr.children().is("img[class^=figure_ ]")) {
            angles[i] = sqr[0];
        }
    }
    return angles;
}

function Check_Angle(elemX, elemY, currentFig_X, currentFig_Y, distanceX, distanceY) {
    return ((elemX == currentFig_X + distanceX) && (elemY == currentFig_Y + distanceY))
}
function KillModel(killer, freeSpaces, victimes) {
    this.killer = killer;
    this.freeSpaces = freeSpaces;
    this.victimes = victimes;
}