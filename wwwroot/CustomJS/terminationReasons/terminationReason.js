

let addTerminationReason = function (){

    $('.modalTitle').text("Add Termination Reason");
    $('.submitEditedRecordBtn').css("display", "none");
    $('.SubmitBtn').css("display", "block");
    $('.code').val("");
    $('.description').val("");
    $('.status').css("display", "none");
    $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
}

let editTerminationReason = function (currentObject){

    let currentRow = currentObject.closest('tr');
    let jobCode = $.trim(currentRow.find('td:eq(0)').text());
    let description = $.trim(currentRow.find('td:eq(1)').text());

    $('.code').val(jobCode).prop("readonly",true);
    $('.description').val(description);
    $('.modalTitle').text("Edit Termination Reason");
    $('.SubmitBtn').css("display", "none");
    $('.status').css("display", "block");
    $('.submitEditedRecordBtn').css("display", "block");
    $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
}

let validateCode = function (){

    let code = $.trim($('.code').val());

    if(code.length > 5){
        $('#code').val('');
        swal({
            title : 'ERROR',
            type : 'error',
            text : 'Code should have a maximum of 5 characters'
        })
        return false;
    }
}

let submitNewTerminationReason = function () {

    let code = $.trim($('#code').val().toUpperCase());
    let description = $.trim($('#description').val());

    if (code === '' || description === '') {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Provide both code and description'
        })
    } 
    else {

        let payload = {
            "Code": code,
            "Description": description,
        };
        $.post("/TerminationReason/AddTerminationReason", {terminationReason: payload})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
               console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'There was an error while processing your request'
                })
            });
    }
}

let submitEditedTerminationReason = function () {

    let code = $.trim($('#code').val().toUpperCase());
    let description = $.trim($('#description').val());
    let status = $.trim($('#status').val());

    if (code === '' || description === '' || status === '') {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Please complete all mandatory fields'
        })
    }
    else {

        let payload = {
            "Code": code,
            "Description": description,
            "Status": status,
        };
        $.post("/TerminationReason/EditTerminationReason", {terminationReason: payload})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'There was an error while processing your request'
                })
            });
    }
}