

$(function (){
    initialiseDatePicker('startDate');
})

let loadCreatePositionModal= function (currentObject){
    
    let currentRow = currentObject.closest('tr');
    let jobCode = $.trim(currentRow.find('td:eq(1)').text());
    let shortDesc = $.trim(currentRow.find('td:eq(2)').text());
    let longDesc = $.trim(currentRow.find('td:eq(3)').text());
    
    $('#jobTitleCode').val(jobCode);
    $('#shortDescription').val(shortDesc);
    $('#longDescription').val(longDesc);
    $('#numOfPositionCodes').val('');
    $('#positionCodes').val('');
    $('#CreatePositionCodeModal').modal({backdrop: 'static', keyboard: false});
}

let createPositionCodes = function(){
    
    let jobTitleCode =     $.trim($('#jobTitleCode').val())
    let numOfPositionCodes = Number($.trim($('#numOfPositionCodes').val()));
    let positionCodes = $('#positionCodes');
    
    if(jobTitleCode === ''){
        swal({title : 'ERROR', type: 'error', text : 'Job code not provided'});
        positionCodes.val('');
        return;
    }
    if(jobTitleCode === '' || numOfPositionCodes <= 0 || numOfPositionCodes > 5){
        swal({title : 'ERROR', type: 'error', text : 'Please provide a valid number of position codes you want to generate. The maximum number at a time is 5'});
        positionCodes.val('');
        return;
    }
    $.post('/PositionCodes/CreatePositionCode',{numOfPositionCodes : numOfPositionCodes,jobTitleCode : jobTitleCode })
        .done(function (response){
            console.log(response);
            
            if(response.success){
                positionCodes.val('');
                positionCodes.val(response.payload);
                $('.positionCodeLabel').css('display','block');
                $('.addPositionCodesBtn').css('display','block');
            }
        })
        .fail(function (error){
            console.log(error);
            swal({
                title : 'ERROR',
                type: 'error',
                text : 'An error occured while processing your request. Please try again after sometime'
            })
        })
}

let submitCreatedPositionCodes = function (){

    let jobCode = $.trim($('#jobTitleCode').val());
    let positionCode = $.trim($('#positionCodes').val());
    let departmentId = Number($.trim($('#department').val()));
    let reportsTo = $.trim($('.reportsToPositionCode').val());

    if (positionCode === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please provide position code(s)'});
        return;
    }
    if (departmentId === 0) {
        swal({title: 'ERROR', type: 'error', text: 'Please select the department'});
        return;
    }

    let payload = {
        'JobTitleCode': jobCode,
        'PositionCode': positionCode,
        'DepartmentId': departmentId,
        'ReportsToPositionCode': reportsTo,
    };

    swal({
        title: 'Are you sure you want to create these position code(s)?',
        text: '',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmationBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
        closeOnConfirm: true,
    });
    $('.confirmationBtn').on('click', function () {
        $.post('/PositionCodes/SaveCreatedPositionCode', {request: payload})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'An error occured while processing your request. Please try again after sometime'
                })
            })
    });
}
