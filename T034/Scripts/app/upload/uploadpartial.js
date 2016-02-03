define(['ckeditoradapter'], function () {

    return {
        Initialize: function () {
            $(".returnImage").click("click", function (e) {
                var urlImage = $(this).attr("data-url");
                window.opener.updateValue("cke_72_textInput", urlImage);
                window.close();
            });
        }
    }
});
