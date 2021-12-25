let loadLeaveEntitlementModal = function () {
    $('#leaveEntitlementDetailsModal').modal({backdrop: 'static', keyboard: false});
}

let getLeaveEntitlementDetails = function () {

    let empCode = $.trim($('.employeeCode').val());
    
    if (empCode === '' && empCode !== null) {
        return;
    }
    
    $.post("/LeaveManagement/GetEmployeeLeaveDetail", {employeeCode: empCode})
        .done(function (response) {
             console.log(response);
            if (!response.success) {
                swal({
                    title: 'ERROR',
                    type: 'error',
                    text: response.message
                })
                return;
            }
            if (response.payload.length === 0) {
                swal({
                    title: 'No Data Found',
                    type: 'info',
                    text: 'No leave entitlement details found for this employee'
                }).then(function () {
                    loadLeaveEntitlementModal();
                })
                return;
            }
            showLeaveEntitlementData(response.payload);

        })
        .fail(function (error) {
            console.log(error);
            swal({
                title: 'ERROR',
                type: 'error',
                text: 'Error occured while processing your request. Please try again.'
            });
        })
}

let showLeaveEntitlementData = function (data) {

    let tableBody = $('.leaveEntitlementTbody');
    tableBody.empty();

    $.each(data, function (index, item) {
        
        let value = item.monetaryValue === null ? 'N/A' : Intl.NumberFormat('en-US').format(item.monetaryValue.toFixed(2));
        
        tableBody.append(`<tr>
           <td>${item.leaveAccrualStartDate}</td>
           <td>${item.leaveTypeDescription}</td>
           <td>${item.entitlement}</td>
           <td>${item.leaveBalance}</td>
           <td>${value}</td>
           </tr>`);
    });
    loadLeaveEntitlementModal();
}
