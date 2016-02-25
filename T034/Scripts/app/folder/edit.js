define(['jquery.fileupload-ui'], function () {

    return {
        Initialize: function () {
         
            /*jslint unparam: true */
            /*global window, $ */
            $(function () {
                'use strict';
                // Change this to the location of your server-side upload handler:
                //var url = '@Url.Content(@Model.Url)';
                var url = '/Folder/UploadFile';
                $('#fileupload').fileupload({
                    uploadTemplateId: null,
                    downloadTemplateId: null,
                    url: url,
                    dataType: 'json',
                    paramName: 'paramName',
                    done: function (e, data) {
                        $.each(data.result, function (index, file) {
                            $('<div class="alert alert-success" role="alert"/>').text(file.name).appendTo('#files');
                        });
                    },
                    fail: function (e, data) {
                        $.each(data.files, function (index, file) {
                            $('<div class="alert alert-danger" role="alert"/>').text(file.name + ' ошибка').appendTo('#files');
                        });
                    },
                    progressall: function (e, data) {
                        var progress = parseInt(data.loaded / data.total * 100, 10);
                        $('#progress .progress-bar').css(
                        'width',
                        progress + '%'
                    );
                    }
                }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
            });

        }
    }
});
