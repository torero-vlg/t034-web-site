define([], function () {

    function setStyle(styleName) {
        $.ajax({
            data: { styleName: styleName },
            url: "/Style/Set"
        });
    }

    return {
        Initialize: function () {

            $("#lowVisionButton").click(function () {
                setStyle("LowVision");
            });

            $("#generalStyleButton").click(function () {
                setStyle("General");
            });
        }
    }
});




