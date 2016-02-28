define(['jquery.fileupload'], function () {

    return {
        Initialize: function () {


            var url = '/Folder/UploadFile';
            var folderId = $('#Id').val();

            $('#fileupload').fileupload({
                url: url,
                dataType: 'json',
                paramName: folderId,
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

        }
    }
});
