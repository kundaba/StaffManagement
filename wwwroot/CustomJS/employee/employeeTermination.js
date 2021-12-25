
let loadTerminationModal = function (){
    
    swal({
        title: 'If you are sure you want to terminate this employee, enter their termination date below',
        buttonsStyling: false,
        html: `<input type ='text' class='form-control' id='terminationDate' style="margin-top: 10px">
                <br>
                 <select class="form-control terminationReason seleckpicker"  id="terminationReason" data-live-search="true">
                 <option value="" selected disabled>Select termination reason</option>
                </select>`,
        confirmButtonClass: 'btn btn-success confirmButton',
        confirmButtonText: 'Submit',
        allowOutsideClick: false,
        cancelButtonClass: 'btn btn-danger cancelButton',
        showCancelButton: true,
    });
    $('.confirmButton').on('click',function () {
        let employeeCode = $.trim($('.employeeCode').val());
        let positionCode = $.trim($('#position').val());
        let terminationDate = $('#terminationDate').val();
        let terminationReasonId = Number($('#terminationReason').val());
        terminateEmployee(employeeCode, terminationDate,terminationReasonId, positionCode);
    });
    initialiseDatePicker('terminationDate');
    getLookupData("TerminationReasons", "terminationReason");
}

let terminateEmployee = function (employeeCode, terminationDate, terminationReason, positionCode){

    if (employeeCode === '') {
        swal({type: 'error', title: 'ERROR', text: 'Employee code not provided'});
        return;
    }
    if (terminationDate === '') {
        swal({type: 'error', title: 'ERROR', text: 'Please provide termination date'});
        return;
    }
    if (new Date(terminationDate) > new Date(new Date().toDateString())) {
        swal({type: 'error', title: 'ERROR', text: 'For this option, termination date can not be in future'});
        return;
    }
    if (terminationReason === 0) {
        swal({type: 'error', title: 'ERROR', text: 'Please provide termination reason'});
        return;
    }
    let payload = {
        "EmployeeCode": employeeCode,
        "PositionCode": positionCode === '' || positionCode === null ? '' : $.trim(positionCode.split('-')[0]),
        "TerminationReason": terminationReason,
        "TerminationDate": new Date(terminationDate).toISOString()
    };
     submitTerminationRequest(payload);
}

let submitTerminationRequest = function (payload){
    $.post('/PromotionAndTermination/TerminateEmployee', {terminationRequest: payload})
        .done(function (response) {
            swAlert(response);
        })
        .fail(function (error) {
            console.log(error);
            swal({type: 'error', title: 'ERROR', text: 'There was an error while processing your request'});
        })
}

let reinstateEmployee = function (){
    swal({
        title: 'Are you sure you want to re-instate this employee?',
        confirmButtonClass: 'btn btn-success confirmButton',
        confirmButtonText: 'Reinstate',
        allowOutsideClick: false,
        cancelButtonClass: 'btn btn-danger cancelButton',
        showCancelButton: true,
    });
    $('.confirmButton').on('click',function () {
        
            $.post('/PromotionAndTermination/ReinstateEmployee', {employeeCode: $.trim($('.employeeCode').val())})
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    console.log(error);
                    swal({type: 'error', title: 'ERROR', text: 'There was an error while processing your request. Try again after sometime'});
                })
    });
}

