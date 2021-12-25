let checkAll = function () {
    $('#datatables tr.tblRow.records').each(function () {
        if (!$(this).prop("disabled", true)) {
            $("input[type='checkbox']").not(this).prop('checked', $(this).checked);
        }
    });
    $('input[type="checkbox"]').not(this).prop('checked', this.checked);
}

let approveTimesheet = function (btnValue) {
    let selectedRecords = [];

    $('input[name="records"]:checked').each(function () {
        selectedRecords.push(this.value);
    });

    if (selectedRecords.length === 0) {
        swal({title: 'ERROR', type: 'error', text: 'Select at least one or more records'});
        return;
    }
    if (btnValue === '') {
        swal({title: 'Unknown action', type: 'error', text: 'Allowed actions are Approve or Reject'});
    } else {
        swal({
            title: 'System Response',
            text: "Are you sure you want to post the selected records?",
            type: 'info',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmPostBtn',
            cancelButtonClass: 'btn btn-danger cancelBtn',
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            buttonsStyling: false
        });
        $('.confirmPostBtn').click(function () {
            submitSelectedRecords(selectedRecords, btnValue);
        });
        $('.cancelBtn').click(function () {
            $('input[name="records"]').prop("checked", false);
        });
    }

}

function submitSelectedRecords(timesheetIdArray, action) {

    $('.btn').prop('disabled', true);
    $.post("/Timesheet/ApproveTimesheet/", {idList: timesheetIdArray, action: action}, "json")
        .done(function (response) {

            if (!response.success) {
                $('.btn').prop("disabled", false);
                swal({title: 'ERROR', type: 'error', text: response.message})
                return;
            }
            swAlert(response);
        })
        .fail(function (error) {
            $('.btn').prop("disabled", false);
            console.log(error);
            swal({title: 'ERROR', type: 'error', text: 'There was an error processing your request'});
        });

}