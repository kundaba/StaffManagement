
$(function () {
    initialiseDatePicker('currentPositionEndDate');
    initialiseDatePicker('newPositionStartDate');
})

let loadPromotionModal = function () {

    let position = $.trim($('#position').val());
    $('#currentPosition').val(position);
    $('#promotionAndTransferModal').modal({backdrop: 'static', keyboard: false});
}

let loadLinkToPositionModal = function () {
    let employeeCode = $.trim($('.employeeCode').val());
    $('#empCode').val(employeeCode);
    $('#linkEmployeeToPositionModal').modal({backdrop: 'static', keyboard: false});
}

let linkToPosition = function (){

    let empCode = $.trim($('#empCode').val());
    let positionCode = $.trim($('#positionCode').val());

    if (positionCode === '' && empCode === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please complete all the mandatory fields'});
        return;
    }

    swal({
        title: 'Are you sure you want to link the employee to this position?',
        text: '',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
        buttonsStyling: false,
        allowOutsideClick: false,
    });
    $('.confirmBtn').click(function () {

        $.post('/PositionCodes/LinkEmployeeToPositionCode', {employeeCode: empCode, positionCode: positionCode})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'An error occured while processing your request. Please try again after sometime'
                });
            })
    });
}

let calculatePromotionStartDate = function (){
    let currentPositionEndDate = $('#currentPositionEndDate').val();
    
    if(currentPositionEndDate !== '' && currentPositionEndDate !== null){
        const dayInMilliSeconds = 86400000;
        let selectedDate = new Date(currentPositionEndDate);
        let followingDay = new Date(selectedDate.getTime() + dayInMilliSeconds );
        let startDate = followingDay.toLocaleDateString();
        $('#newPositionStartDate').val(startDate);
    }
}

let submitPromotionDetails = function (){
    
    let employeeCode = $.trim($('.employeeCode').val());
    let currentPosition = $.trim($('#currentPosition').val());
    let endDate = $('#currentPositionEndDate').val();
    let newPositionCode = $.trim($('#newPosition').val());
    let startDate = $('#newPositionStartDate').val();
    
    if(currentPosition === '' || endDate ==='' || newPositionCode ==='' || startDate === ''){
        swal({title: 'ERROR', type: 'error', text: 'Please complete all the mandatory fields'});
        return;
    }
    swal({
        title: 'Are you sure you want to effect this promotion?',
        text: '',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmationBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
    });
    $('.confirmationBtn').on('click', function () {

        let payload = {
            "EmployeeCode": employeeCode,
            "OldPositionCode": $.trim(currentPosition.split('-')[0]),
            "EndDate": new Date(endDate).toISOString(),
            "NewPositionCode": $.trim(newPositionCode.split('-')[0]),
            "StartDate": new Date(startDate).toISOString(),
        }

        $.post("/PromotionAndTermination/PromoteEmployee", {promotionRequest: payload})
            .done(function (response) {
                swAlert(response)
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'Error occured while processing your request. Please try again later.'
                });
            })
    });
}