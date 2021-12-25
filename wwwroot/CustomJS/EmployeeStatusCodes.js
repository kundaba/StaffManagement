
$(document).ready(function (){

    getEmployeeStatus("EmployeeStatus");

    function getEmployeeStatus(dataCategory) {
        $.post("/Parameters/GetLookupData", { dataCategory: dataCategory}, "json")
            .done(function (data) {
                
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        $(".employeeStatus").append("<option value = " + item.id + " > " + item.description + "</option >").selectpicker('refresh');
                    });
                }
            })
            .fail(function (error) {
                console.log(error);
            });
    }
})