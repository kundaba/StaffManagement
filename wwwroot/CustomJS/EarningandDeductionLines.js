

$(document).ready(function () {
   // GetEarningLines();
    //GetDeductionLines();

    function GetEarningLines() {
        $.post("/PayslipDefintion/EarningLines", "json")
            .done(function (data) {
                if (data !== null) {
                    $(".tbodyEarningLines").empty();
                    $.each(data, function (i, item) {
                        $(".tbodyEarningLines").html("<tr class='earningstblRow'>" +
                            "<td>" + item.earningLineCode + "</td>" +
                            "<td>" + item.earningLineDescription + "</td>" +
                            "<td><select class='form-control' name='earningType' id='earningType'>" +
                            "<option value='0' selected disabled>Select Type</option>" +
                            "<option value='Percentage'>Percentage </option>" +
                            "<option value='FixedAmount'>Fixed Amount</option>" +
                            "</select></td >" +
                           "<td><input type ='text' class='form-control Amount'></td></tr>");
                    });
                }
            })
            .fail(function (error) {
                console.log(error);
            });
    }
    function GetDeductionLines() {
        $.post("/PayslipDefintion/DeductionLines", "json")
            .done(function (data) {
                if (data !== null) {
                    $(".tbodyDeductionLines").empty();
                    $.each(data, function (i, item) {
                        $(".tbodyDeductionLines").append("<tr>" +
                            "<td>" + item.deductionCode + "</td>" +
                            "<td>" + item.deductionDecsription + "</td>" +
                            "<td><select class='form-control' name='deductionType' id='deductionType'>"+
                            "<option value='0' selected disabled>Select Type</option>" +
                            "<option value='Percentage'>Percentage </option>" +
                            "<option value='FixedAmount'>Fixed Amount</option>" +
                            "</select></td >" +
                            "<td><input type ='text' class='form-control value'></td></tr>"
                        );
                    });
                }
            })
            .fail(function (error) {
                console.log(error);
            });
    } 
})