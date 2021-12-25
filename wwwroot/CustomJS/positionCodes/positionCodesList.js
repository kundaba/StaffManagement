
let myapp = angular.module('myapp', []);

  myapp.controller('jobCodesController', function ($scope, $http) {

    $http.post('/PositionCodes/GetPositionCodes').then(function (response) {
        
        if (!response.data.success) {
            swal({
                title: 'ERROR',
                type: 'error',
                text: 'failed to load position codes listing'
            })
            return;
        }
        if (response.data.payload.length > 0) {
            $scope.result = response.data.payload;
            setTimeout(function () {
                initializeDatatable('datatable');
            }, 300);
        }

    }, function errorCallback(error) {
        console.log(error);
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Error occured while processing your request'
        });
    });
});

let showLinkPositionToSupervisorModal = function (currentObject){
    
    let currentRow = currentObject.closest('tr');
    let positionCode = $.trim(currentRow.find('td:eq(0)').text());
    
    $('#posCode').val(positionCode);
    $('#positionCode').val('');
    $('#linkPositionToSupervisorModal').modal({backdrop: 'static', keyboard: false});
}

let linkPositionToSupervisor = function (){

    let positionCode = $.trim($('#posCode').val());
    let reportsTo = $.trim($('#positionCode').val());
    let reportsToCode = $.trim(reportsTo.substring(0, 4));

    if (positionCode === '') {
        swal({type: 'error', title: 'ERROR', text: 'Position code not provided'});
        return;
    }
    if (reportsTo === '') {
        swal({type: 'error', title: 'ERROR', text: 'Reports-To not provided'});
        return;
    }
    if (reportsToCode === positionCode.substring(0,4)) {
        $('#positionCode').val('');
        swal({type: 'error', title: 'ERROR', text: 'ReportsTo position can note be the same as the position you are linking'});
        return;
    }

    swal({
        title: 'Are you sure you want to link this position to the selected reports to?',
        text: '',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmationBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
    });
    $('.confirmationBtn').on('click', function () {
        $.post('/PositionCodes/LinkPositionToSupervisor', {positionCode: positionCode, reportsTo: reportsTo})
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