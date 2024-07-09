define(['layout'], function (layout) {

    function backup() {
        $.ajax({
            url: '/Setting/Backup',
            method: 'POST',
            data: {

            }
        })
            .complete(function (data) {
                layout.showResult(data);
            });
    }

    return {
        Initialize: function () {
            $("#btnBackup").click(function () {
                backup();
            });
        }
    }
});
