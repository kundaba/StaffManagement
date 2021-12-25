
$(document).ready(function () {

    let employeeNumber = "";
    let id = $("#search");
    id.val("");
    id.autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Employee/AutoCompleteSearch',
                dataType: "json",
                method: "POST",
                data: {
                    searchTerm: id.val(),
                },
                success: function (data) {
                    if (data.length > 0) {
                        response(data);
                    }
                    else {

                        response([{ label: 'employee not found.' }]);
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                },
            });
        },
        min_length: 2,
        appendTo: (".searchDiv")
    });

    $(".search").click(function () {
        if ($("#search").val() !== '') {
            employeeNumber = $("#search").val();
            if (employeeNumber !== null && employeeNumber !== "") {
                Search(employeeNumber);
            }
        }
        else {
            alertMessage('Please enter a search string');
        }
    });

    function Search(empNo) {
        $.post("/Employee/GetEmployeeById/", { employee: empNo},'json')
            .done(function (data) {
                if (data !== null) {
                    $(".material-datatables").css("display", "block");
                    $(".PayrollLines").css("display", "block");
                    $(".tbody").empty();
                    $(".tbody").append("<tr>" +
                        "<td>"+ data.employeeCode+"</td>" +
                        "<td>" + data.firstName +"</td>" +
                        "<td>" + data.lastName +"</td>" +
                        "<td>" + data.jobTitle + "</td>" +
                        "<td>" + data.department + "</td>" +
                        "<td>" + data.employeeStatus +"</td></tr>");
                }
                else {
                    $(".material-datatables").css("display", "none");
                    $(".PayrollLines").css("display", "none");
                    alertMessage("No record found");
                }              
            })
            .fail(function (error) {
                console.log(error);
            })
    }
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