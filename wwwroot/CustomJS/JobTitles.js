$(document).ready(function () {
    
    $('.SubmitBtn').on('click', function () {

        let jobCode = $.trim($('.JobCode').val().toUpperCase());
        let shortDesc = $.trim($('.ShortDescription').val());
        let longDesc = $.trim($('.LongDescription').val());
        let gradeId = Number($('#grade').val());

        if (!(jobCode !== "" && shortDesc !== "" && longDesc !== "" && gradeId !== 0)) {
            alertMessage("Please complete all mandatory fields");
        } else {

            $(".btn").prop("disabled", true);
            let model = {
                JobCode: jobCode,
                ShortDescription: shortDesc,
                LongDescription: longDesc,
                JobGradeId: gradeId,
            };
            $.post("/Parameters/AddJobTitle", {model: model})
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    alertMessage(error.responseText);
                });
        }
    });

    $('.submitEditedRecordBtn').on('click', function () {
        
        let id = Number($('.JobTitleId').text());
        let jobCode = $.trim($('.JobCode').val());
        let shortDesc = $.trim($('.ShortDescription').val());
        let longDesc = $.trim($('.LongDescription').val());
        let status = $.trim($('#Status').val());

        if (!(id !== 0 && shortDesc !== "" && longDesc !== "" && status !== "")) {
            alertMessage("Please complete all mandatory fields");
        } else {
            swal({
                title: 'Action Confirmation',
                text: 'Are you sure you want to edit this record?',
                type: 'info',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmEditBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $(".confirmEditBtn").click(function () {
                submitEditedRecord(id, jobCode, shortDesc, longDesc, status);
            });
        }
    });
     
    let submitEditedRecord = function (id,jobCode,shortDescription, longDescription,status) {
       
        $('.btn').prop('disabled', true);
        let editedModel = {
            JobTitleID: id,
            JobCode : jobCode,
            ShortDescription: shortDescription,
            LongDescription: longDescription,
            Status: status,
            JobGradeId: 0,
        };
        $.post('/Parameters/EditJobTitle', { model: editedModel })
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                alertMessage(error.responseText);
            });
    }
    
    let alertMessage = function (msg) {
        swal({
            title: "ERROR",
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info"
        });
    }
})


let addNewJobTitle = function (){

    $('.modalTitle').text("Add JobTitle");
    $('.submitEditedRecordBtn').css("display", "none");
    $('.SubmitBtn').css("display", "block");
    $('.JobCode').val("");
    $('.ShortDescription').val("");
    $('.LongDescription').val("");
    $('.Status').css("display", "none");
    $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
}

let editJobTitle = function (currentObject){

    let currentRow = currentObject.closest('tr');
    let id = Number(currentRow.find('td:eq(0)').text());
    let jobCode = $.trim(currentRow.find('td:eq(1)').text());
    let shortDescription = $.trim(currentRow.find('td:eq(2)').text());
    let longDescription = $.trim(currentRow.find('td:eq(3)').text());

    $('.JobCode').val(jobCode).prop("readonly",true);
    $('.ShortDescription').val(shortDescription);
    $('.LongDescription').val(longDescription);
    $('.JobTitleId').text(id);
    $('.modalTitle').text("Edit JobTitle");
    $('.SubmitBtn').css("display", "none");
    $('.gradeDiv').css("display", "none");
    $('.Status').css("display", "block");
    $('.submitEditedRecordBtn').css("display", "block");
    $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
}

let validateJobCode = function (){

    let jobCode = $.trim($('.JobCode').val());
    
    if(jobCode.length < 4 || jobCode.length > 4){
        $('.JobCode').val('');
        swal({
            title : 'ERROR',
            type : 'error',
            text : 'Job code should have 4 characters only'
        })
        return false;
    }
}