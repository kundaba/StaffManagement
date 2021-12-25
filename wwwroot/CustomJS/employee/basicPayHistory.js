
let myapp = angular.module('myapp', []);

myapp.controller('basicPayHistoryController', function ($scope, $http) {

    $('#basicPayHistoryBtn').on('click', function () {

        let empCode = $.trim($('.employeeCode').val());
        getBasicPayDetails($scope, $http, empCode);
        $('#basicPayHistoryModal').modal({backdrop: 'static', keyboard: false});
    });
    
    $('#bankDetailsBtn').on('click', function () {

        let empCode = $.trim($('.employeeCode').val());
        getBankDetails($scope, $http, empCode);
        $('#bankDetailsModal').modal({backdrop: 'static', keyboard: false});
    })
    
});

let getBasicPayDetails = function ($scope, $http, employeeCode) {

    $http({
        url: "/BasicPayAmendment/GetBasicPayHistory",
        method: 'POST',
        params: {
            employeeCode: employeeCode
        }
    }).then(function (response) {

        if (!response.data.success) {
            swal({
                title: 'ERROR',
                type: 'error',
                text: 'failed to load basic pay history'
            })
            return;
        }
        $scope.result = response.data.payload;
    }, function (error) {
        console.log(error);
    });
  
}

let loadBasicPayAmendmentModal = function () {

    initialiseDatePicker('startDate');
    $('#basicPayAmendmentModal').modal({backdrop: 'static', keyboard: false});
}

let submitAmendedBasicPay = function () {

    let newBasicPay = Number($.trim($('#newBasicPay').val()));
    let reason = $.trim($('#reason').val());
    let startDate = $.trim($('#startDate').val());

    if (newBasicPay <= 0) {
        swal({title: 'ERROR', type: 'error', text: 'Basic pay can not be zero or below'})
        return;
    }
    if (reason === '' || startDate === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please provide the reason and start date'});
        return;
    }
    if (new Date(startDate) > new Date(new Date())) {
        swal({title: 'ERROR', type: 'error', text: 'Start date can not be in future'});
    } else {
        swal({
            title: 'Are you sure you want to amend this basic pay?',
            text: '',
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmationBtn',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: "Yes",
            closeOnConfirm: true,
        });
        $('.confirmationBtn').on('click', function () {

            let payload = {
                "EmployeeCode": $.trim($('.employeeCode').val()),
                "RemunerationAmount": newBasicPay,
                "StartDate": new Date(startDate).toISOString(),
                "Reason": reason,
            }

            $.post("/BasicPayAmendment/UpdateEmployeeRemuneration", {request: payload})
                .done(function (response) {
                    if (!response.success) {
                        swal({
                            title: 'ERROR',
                            type: 'error',
                            text: response.message
                        })
                        return;
                    }
                    swAlert(response)
                })
                .fail(function (error) {
                    console.log(error);
                    swal({
                        title: 'ERROR',
                        type: 'error',
                        text: 'Error occured while processing your request. Please try again.'
                    });
                })
        });
    }
}