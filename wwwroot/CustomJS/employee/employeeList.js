
let myapp = angular.module('myapp', []);

myapp.controller('employeeListController', function ($scope, $http) {

    $http.post('/Employee/GetEmployeesList').then(function (response) {

        if (!response.data.success) {
            swal({
                title: 'ERROR',
                type: 'error',
                text: 'failed to load employee listing'
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