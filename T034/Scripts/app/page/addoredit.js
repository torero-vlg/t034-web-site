define(['ckeditoradapter'], function () {

    function updateValue(id, value) {
        // this gets called from the popup window and updates the field with a new value
        var jqueryObjectByClass = $(".cke_dialog_ui_input_text")[1];
        jqueryObjectByClass.value = value;
    };

    window.updateValue = updateValue;

    return {
        Initialize: function () {
            CKEDITOR.replace('content', {
                filebrowserImageBrowseUrl: '/Upload/UploadPartial',
                filebrowserImageUploadUrl: '/Upload/Uploadnow',

                filebrowserBrowseUrl: '/Upload/UploadPartial',
                filebrowserUploadUrl: '/Upload/Uploadnow'
            });
        }
    }
});
