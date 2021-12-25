let addLeaveTypeMethods = {

    loadLeaveTypeModal : function (){
        
        $('.code').val('');
        $('.description').val('');
        $('.gender').val('');
        $('.entitlement').val('');
        $('#AddLeaveTypeModal').modal({backdrop: 'static', keyboard: false}); 
    },
    
    addLeaveType: function () {

        let code = $.trim($('#code').val());
        let description = $.trim($('#description').val());
        let gender = $.trim($('#gender').val());
        let entitlement = Number($('#entitlement').val());
        let cycle = $.trim($('#cycle').val());
        let balanceBroughtForwardOption = $.trim($('#balanceBroughtForwardOption').val());

        if (code <= '' || code.length > 8) {
            swal({title: 'ERROR', type: 'error', text: 'Provide valid code'})
            return;
        }
        if (description === '') {
            swal({title: 'ERROR', type: 'error', text: 'Please provide leave type description'});
            return;
        }
        if (gender === '') {
            swal({title: 'ERROR', type: 'error', text: 'Select gender'});
            return;
        }
        if (entitlement <= 0) {
            swal({title: 'ERROR', type: 'error', text: 'Provide a valid value for entitlement'});
            return;
        }
        if (cycle === '') {
            swal({title: 'ERROR', type: 'error', text: 'Provide select cycle'});
        }else {
            swal({
                title: 'Are you sure you want to add this leave type?',
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
                    "Code": code.toUpperCase(),
                    "LeaveTypeDescription": description,
                    "Entitlement": entitlement,
                    "ApplicableGender": gender,
                    "Cycle": cycle,
                    "BalanceBroughtForwardOption": balanceBroughtForwardOption
                    
                };
                addLeaveTypeMethods.saveLeaveType(payload);
            });
        }
    },

    saveLeaveType: function (payload) {

        $.post("/LeaveManagement/AddLeaveType", {request: payload})
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
    }
}




