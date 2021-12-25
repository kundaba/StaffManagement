function swAlert(response){

    let title =  'Error';
    let alertType = 'error';
    let message = 'Error occured while processing your request. Try again later';

    if (response.success)
    {
        title =  'Successful';
        alertType = 'success';
        message = response.message;
    }
    if(!response.success){
        title =  'ERROR';
        alertType = 'error';
        message = response.message;
    }

    swal({
        title: title,
        text: message,
        type: alertType,
        confirmButtonClass: 'btn btn-success',
        confirmButtonText: "Ok",
        buttonsStyling: false,
        closeOnConfirm: true
    }).then(function () {
        window.location.reload(true);
    });
}