$(document).ready(function () {
    function alertMessage(msg) {
        swal({
            title: "System Response",
            text: msg,
            type: "info",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn"
        }).then(function () {
            window.location.reload(true);
        });
    }
})