let loadCaseSubmissionModal = function () {

    initialiseDatePicker('dateOfOffence');
    $('#employeeCode').val('');
    $('#dateOfOffence').val('');
    $('#caseDescription').val('');
    $('#disciplinaryCaseSubmissionModal').modal({backdrop: 'static', keyboard: false});
}

let submitDisciplinaryCase = function () {

    let employeeCode = $.trim($('.employeeCode').val());
    let caseDescription = $.trim($('#caseDescription').val());
    let caseType = $.trim($('#caseType').val());
    let category = $.trim($('#category').val());
    let caseOutcome = $.trim($('#caseOutcome').val());
    let date = $('#dateOfOffence').val();

    if (employeeCode === '' || caseDescription === '' || caseType === '' || category === '' || caseOutcome === '' || date === '') {
        swal({title: 'ERROR', type: 'error', text: 'Complete all mandatory fields'});
        return;
    }
    if (new Date(date) > new Date()) {
        swal({title: 'ERROR', type: 'error', text: 'Date can not be in future'});
        return;
    }
    swal({
        title: 'Are you sure you want to submit this case?',
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
            "EmployeeCode": employeeCode.toUpperCase(),
            "DateOffenceCommitted": new Date(date).toISOString(),
            "CaseType": caseType,
            "Category": category,
            "CaseOutcome": caseOutcome,
            "CaseDescription": caseDescription
        };
       submit(payload);
    });
}

let submit = function (payload){
    
    $('.btn').prop("disabled", true);
    $.post("/DisciplinaryCases/AddDisciplinaryCase/", {request: payload})
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
}