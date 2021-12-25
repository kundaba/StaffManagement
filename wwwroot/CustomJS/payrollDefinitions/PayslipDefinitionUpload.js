
let uploadFile = function (lineId){

    let file = lineId === 1 ? $('.earningLinesFile').val(): $('.deductionLinesFile').val();
     
    if (file === '') {
        swal({
            title: 'ERROR',
            text: 'Please select the file to upload',
            type: 'error',
        });
        return;
    }
    swal({
        title: 'Are You sure',
        text: 'Are you sure you want to upload this file? This action can not be reversed once done.',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        buttonsStyling: false
    });
    $('.confirmBtn').on('click', function () {
        let formData = new FormData();
        formData.append("lineId", lineId);

        if (lineId === 1) {
            $('input[name="earningsFileUpload"]').each(function (a, b) {
                let fileInput = $('input[name="earningsFileUpload"]')[a];
                let files = fileInput.files[a];
                formData.append("file", files);
            });
        } else {
            $('input[name="deductionsFileUpload"]').each(function (a, b) {
                let fileInput = $('input[name="deductionsFileUpload"]')[a];
                let files = fileInput.files[a];
                formData.append("file", files);
            });
        }
        
        submitFile(formData);
    });
}

let submitFile = function (payload) {

    $('.btn').prop('disabled', true);

    $.ajax({
        url: '/BulkUpload/UploadPayslipLines',
        method: 'POST',
        contentType: false,
        processData: false,
        data: payload,

        success: function (response) {
            
            if(!response.success){
                $('.btn').prop('disabled', false);
                swal({title: 'ERROR', text: response.message, type: 'error'});
                return;
            }
           swAlert(response);
        },
        error: function (xhr, i, exception) {
            $('.btn').prop('disabled', false);
            console.log(exception);
            swal({
                title: 'ERROR',
                text: exception,
                type: 'error',
                buttonsStyling: false,
                confirmButtonClass: 'btn btn-info okBtn'
            });
        },
    });
}