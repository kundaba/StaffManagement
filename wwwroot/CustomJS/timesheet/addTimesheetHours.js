
let loadTimesheetModal = function () {

    initialiseDatePicker('date');
    $('#empCode').val('');
    $('#date').val('');
    $('#numberOfHours').val('');
    let employeeNo = $('.employeeCode').val();

    if (employeeNo === '') {
        swal({title: 'ERROR', type: 'error', text: 'Employee number not provided'});
        return;
    }
    employeeNo = employeeNo.split(' ')[2];
    $('#empCode').val($.trim(employeeNo.toUpperCase().replace('(','').replace(')','')));
    $('#timesheetHoursModal').modal({backdrop: 'static', keyboard: false});
}

let submitTimesheetHours = function () {

    let employeeCode = $('#empCode').val();
    let hoursWorked = Number($('#numberOfHours').val());
    let date = $('#date').val();

    if (employeeCode === '') {
        swal({title: 'ERROR', type: 'error', text: 'Employee number not provided'});
        return;
    }
    if (date === '') {
        swal({title: 'ERROR', type: 'error', text: 'Date not provided'});
        return;
    }
    if (hoursWorked === 0 || hoursWorked < 0) {
        swal({title: 'ERROR', type: 'error', text: 'Invalid hours provided'});
        return;
    }
    if (new Date(date) > new Date()) {
        swal({title: 'ERROR', type: 'error', text: 'Date can not be in future'});
        return;
    }
    swal({
        title: 'Are you sure you want to submit this?',
        text: "",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmPostBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        buttonsStyling: false
    });
    
    $('.confirmPostBtn').click(function () {
        let payload = {
            "EmployeeCode" : employeeCode.toUpperCase(),
            "HoursWorked" : hoursWorked,
            "Date": new Date(date).toISOString()
        };
        
        $('.btn').prop("disabled", true);
        $.post("/Timesheet/AddTimesheetHours/", {request: payload})
            .done(function (response) {
                if (!response.success) {
                    swal({title: 'ERROR', type: 'error', text: response.message})
                    return;
                }
                swAlert(response);
            })
            .fail(function (error) {
                $('.btn').prop("disabled", false);
                console.log(error);
                swal({title: 'ERROR', type: 'error', text: 'There was an error processing your request'});
            });
    });
}