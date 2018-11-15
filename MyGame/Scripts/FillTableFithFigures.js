function FillTable(model) {
    model.Figures.forEach(function (value, index, array) {
        var ImgSrc = "";
        if (value.Color == "Black") {
            if (value.IsSuperFigure == true) {
                ImgSrc ="../../Images/damkaB.png";
            }
            else {
                ImgSrc = "../../Images/black.png";
            }
            
        }
        else{
            if (value.IsSuperFigure == true) {
                ImgSrc = "../../Images/damkaW.png";
            }
            else {
                ImgSrc = "../../Images/white.png";
            }
        }
        var elem = $(".tableSquare[data-xcoord='" + value.XCoord + "'][data-ycoord='" + value.YCoord + "']");
        elem.html("<img class='figure_" + value.Color + "' src='" + ImgSrc + "' data-fig-id='" + value.Id + "'>");
    });
}