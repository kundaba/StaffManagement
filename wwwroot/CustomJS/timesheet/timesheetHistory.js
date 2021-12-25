
let loadTimesheetHistoryModal = function () {

    initialiseDatePickerByClass('datepicker');
    $('#startDate').val('');
    $('#endDate').val('');
    $('#timesheetHistoryModal').modal({backdrop: 'static', keyboard: false});
}

let getHistoryData = function (){
    
    let dateFrom = $('#startDate').val();
    let dateTo =  $('#endDate').val();

    if (dateFrom === '' || dateTo === '') {
        swal({title: 'ERROR', type: 'error', text: 'Provide both start and end date'});
        return;
    }
    
    $('.btn').prop('disabled', true);
    $.post("/Timesheet/GetTimesheetByPeriod/", {startDate: new Date(dateFrom).toISOString(), endDate: new Date(dateTo).toISOString()})
        .done(function (response) {
            
            if (!response.success) {
                $('.btn').prop("disabled", false);
                swal({title: 'ERROR', type: 'error', text: response.message})
                return;
            }
            if(response.payload === null || response.payload.length === 0){
                $('.btn').prop("disabled", false);
                swal({title: 'No Data Found', type: 'info', text: 'No data found for the provided period'})
                return;
            }
            $('.btn').prop("disabled", false);
            let data = response.payload;
            let tableBody = $('.tblBody');
            tableBody.empty();

            $.each(data, function (index, item){

                tableBody.append(`<tr>
                <td> ${item.employeeCode} </td>
                <td> ${item.firstName} </td>
                <td> ${item.lastName} </td>
                <td> ${item.hoursWorked} </td>
                <td> ${new Date(item.dateWorked).toDateString()} </td>
                <td> ${new Date(item.dateCreated).toDateString()} </td>
                <td> ${item.status} </td>
                </tr>`);
            });
            initializeDatatable('datatable');
        })
        .fail(function (error) {
            $('.btn').prop("disabled", false);
            console.log(error);
            swal({title: 'ERROR', type: 'error', text: 'There was an error processing your request'});
        });
}

$(function (){
    

})

