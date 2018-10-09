function FillTable(figures) {
    figures.forEach(function (value, index, array) {

        var ImgSrc = "../Images/black.png";
        if (value.Color == "White") {
            ImgSrc = "../Images/white.png";
        }
        var elem = $(".tableSquare[data-Xcoord = '" + value.XCoord + "'][data-Ycoord = '" + value.YCoord + "']").html("<img src='" + ImgSrc + "' />");
    });
}