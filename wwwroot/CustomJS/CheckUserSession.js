
$(document).ready(function () {
    
    $(window).on('beforeunload', function () {
        $.post("/UserManager/CheckUserSession")
            .done(function (msg) {
            })
            .fail(function (error) {
                console.log(error);
            });
    });

});