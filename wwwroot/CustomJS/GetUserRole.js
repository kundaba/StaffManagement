$(document).ready(function () {
   
    let userName = $.trim($(".userName").text());
    
    $.post("/UserAccount/GetUserRole/", { username: userName }, "json")
        .done(function (data) {
            
            if (data === "1") {
                $(".AllOptions").css("display", "block");
                $(".UserAdmin").css("display", "block");
            }
            else if (data === "3") {
                $(".AllOptions").css("display", "block");
            }
            else if (data === "2") {
                $(".AllOptions").css("display", "block");
                $(".UserAdmin").css("display", "none");
                $(".ParameterSetup").css("display", "none");
            }
            else {
                $(".AllOptions").css("display", "none");
            }
        })
        .fail(function (error) {
            console.log(error);
            swal({
                title: "ERROR",
                text: 'Failed to Load the user profile',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn",
                allowOutsideClick: false
            }).then(function () {
                window.location.href="/UserAccount/Login";
            });
        });
    


})