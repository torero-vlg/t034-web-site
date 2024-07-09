define([], function () {

    $('#successContainer').hide();
    $('#errorContainer').hide();

    $('.alert').click(function () {
        $(this).hide();
    });

    return {
        Initialize: function () {

            
        },
        showResult: function (data) {

            var response = JSON.parse(data.responseText);

            if (response.Status != 0) {
                $('#errorContainer').show();
                $('#successContainer').hide();
                $('#errorContainer').html(response.Message);
            }
            else {
                $('#errorContainer').hide();
                $('#successContainer').show();
                $('#successContainer').html(response.Message);
            }

            setTimeout(function () {
                $('.alert').hide(1000);
            }, 5000);
        }
    }
});




