
let $myapp = angular.module('bankDetailsApp', []);

$myapp.controller('bankDetailsController', function ($scope, $http) {
    
});

let getBankDetails = function ($scope, $http, employeeCode) {

    $http({
        url: "/BanksDetails/GetEmployeeBankDetails",
        method: 'POST',
        params: {
            employeeCode: employeeCode
        }
    }).then(function (response) {
        if (!response.data.success) {
            swal({
                title: 'ERROR',
                type: 'error',
                text: response.data.message
            })
            return;
        }
        let data = response.data.payload;
        let tableBody = $('.bankDetailsTbody');
        tableBody.empty();
        
        $.each(data, function (index, item){
            
            let isDefaultBank = item.isDefaultBank;
           tableBody.append(`<tr class="bankDetRow">
           <td>${item.bankName}</td>
           <td>${item.branchName}</td>
           <td class="branchCode">${item.branchCode}</td>
           <td class="accountNumber">${item.accountNumber}</td>
           <td class="accountName">${item.accountName}</td>
           <td>${item.statusCode}</td>
           <td><input type="radio" id="defaultBank" name="defaultBank" value="${isDefaultBank}" onclick="changeDefaultBank($(this))"></td>
           </tr>`);
        });
        selectDefaultBank();
        
    }, function (error) {
        console.log(error);
    });
    
}

let selectDefaultBank = function (){
    
    $('.bankDetailsTable tr.bankDetRow').each(function () {
        let isDefaultBankValue = Number($(this).closest('tr').find('td:eq(6)').find(':input').val());
        if(isDefaultBankValue === 1){
            $(this).closest('tr').find('td:eq(6)').find(':input').attr('checked', true).prop('disabled',true);    
        }
    });
}

let loadAdditionalBankAccountModal = function (){
    
    let accountName = $.trim($('.firstName').val()+" "+$('.lastName').val());
    $('#accountName').val(accountName);
    $('#additionalBankAccountModal').modal({backdrop: 'static', keyboard: false});
}

let addNewBankDetails = function () {

    let accountName = $.trim($('#accountName').val());
    let accountNumber = $.trim($('#accountNumber').val());
    let bankId = Number($.trim($('#bank').val()));
    let branchId = Number($.trim($('#branch').val()));

    if (accountName === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please provide account name'})
        return;
    }
    if (accountNumber === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please provide account number'})
        return;
    }
    if (bankId === 0) {
        swal({title: 'ERROR', type: 'error', text: 'Please provide bank name'})
        return;
    }
    if (branchId === 0) {
        swal({title: 'ERROR', type: 'error', text: 'Please provide branch name'})
        return;
    }

    swal({
        title: 'Are you sure you want to add these bank details?',
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
            'EmployeeCode': $.trim($('.employeeCode').val()),
            'AccountName': accountName,
            'AccountNumber': accountNumber
        }

        $.post("/BanksDetails/UpdateBankDetails", {request: payload, bankId: bankId, branchId: branchId})
            .done(function (response) {
                if (!response.success) {
                    swal({
                        title: 'ERROR',
                        type: 'error',
                        text: response.message
                    })
                    return;
                }
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'Error occured while processing your request'
                });
            })
    });
}

let changeDefaultBank = function ($this){
    
    let currentRow = $this.closest('tr');
    let accountNumber = $.trim(currentRow.find('.accountNumber').text());
    let employeeCode = $.trim($('.employeeCode').val());
    
    if (accountNumber === '') {
        swal({title: 'ERROR', type: 'error', text: 'Please provide account number'})
        return;
    }
    swal({
        title: 'Are you sure you want to make this the default account?',
        text: '',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmationBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
        closeOnConfirm: true,
    });
    $('.confirmationBtn').on('click', function () {
        
        $.post('/BanksDetails/ChangeDefaultBank', {employeeCode: employeeCode, accountNumber: accountNumber})
            .done(function (response) {
                if (!response.success) {
                    swal({
                        title: 'ERROR',
                        type: 'error',
                        text: response.message
                    })
                    return;
                }
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: 'Error occured while processing your request'
                });
            })
    });
}