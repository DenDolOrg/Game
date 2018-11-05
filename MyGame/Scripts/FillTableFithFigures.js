function FillTable(figures) {
    figures.forEach(function (value, index, array) {

        var ImgSrc = "../../Images/black.png";
        if (value.Color == "White") {
            ImgSrc = "../../Images/white.png";
        }
        var elem = $(".tableSquare[data-xcoord='" + value.XCoord + "'][data-ycoord='" + value.YCoord + "']");
        elem.html("<img class='figure_" + value.Color + "' src='" + ImgSrc + "' data-fig-id='" + value.Id + "'>");
    });
}