function SetupMove(model, color) {
    var stepHub = $.connection.stepHub,
        $figures = $(".figure_" + color),
        $validBoxe_left,
        $validBoxe_right,
        stepModel
        $Boxes = $(".blackSquares");
    var currentFigure;

    stepHub.client.changePosition = function (step) {
        var newParent = $("blackSquares[data-xcoord='" + step.newX + "'][data-ycoord='" + step.newY + "']");
        $currentFig = $("img[data-fig-id='" + step.figureId + "']");
        var oldParent = currentFig.parent();

        var xOld = oldParent.offset().left;
        var xNew = newParent.offset().left;

        var yOld = oldParent.offset().top;
        var yNew = newParent.offset().top;

        var $figureClone = $currentFig.clone();
        $currentFig.hide();
        newParent.append($currentFig);
        oldParent.append($figureClone);
        $figureClone.animate({
            left: xNew - xOld,
            top: yNew - yOld
        }, "slow", function () {
            oldParent.empty();
            $currentFig.show();
            });

    }
    $.connection.hub.start().done(function () {
        $figures.draggable({
            containment: $("#checkersTable"),
            stop: function () {
                var currentFigure = $(this);
                currentFigure.css("z-index", "10");
                currentFigure.css({ left: 0, top: 0 });
                $validBoxe_left.css("background-color", "#3B363A");
                $validBoxe_right.css("background-color", "#3B363A");
            },
            start: function () {
                currentFigure = $(this);
                currentFigure.css("z-index", "15");
                var validY = currentFigure.parent().data("ycoord") - 1;
                var validX_r = currentFigure.parent().data("xcoord") + 1;
                var validX_l = validX_r - 2;
                $validBoxe_left = $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_l + "']");
                $validBoxe_right = $(".blackSquares:not(:has(img))[data-ycoord ='" + validY + "'][data-xcoord ='" + validX_r + "']");
                $validBoxe_left.css("background-color", "green");
                $validBoxe_right.css("background-color", "green");

            }

        });

        $Boxes.droppable({
            drop: function (event, ui) {
                var box = $(this);
                var col = box.css("background-color");
                if (col == "rgb(0, 128, 0)") {
                    box.append(ui.draggable);
                    stepModel = {
                        newY: box.data("ycoord"),
                        newX: box.data("xcoord"),
                        figureId: ui.draggable.data("fig-id"),
                        gameId: model.GameId
                    }
                    stepHub.server.updateField(stepModel);
                }
            }
        })

        
    });
};
