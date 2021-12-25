let amendmentMethods = {

    loadAmendmentModal: function () {
        $('#employeeAmendmentModal').modal({backdrop: 'static', keyboard: false});
    },

    submitAmendment: function (payload) {

        $.post("/Employee/EmployeeAmendment", {amendmentRequest: payload})
            .done(function (response) {
                swAlert(response);
            }).fail(function (error) {
            console.log(error);
            swal({
                title: 'ERROR',
                type: 'error',
                text: 'Error occured while executing your request. Please try again'
            });
        });
    }
}


$(function () {

    initialiseDatePicker('newValue');

    $('.amendmentButton').on('click', function (e) {
        e.preventDefault();
        let employeeCode = $.trim($('.employeeCode').val());
        $('#employeeCode').text(employeeCode);
        amendmentMethods.loadAmendmentModal();
    });

    $('.addFieldsBtn').on('click', function () {

        $('.fieldsAmendmentTable').append("<tr class='fieldsTableRow'><td>" +
            "<label class='block'>Field Name" +
            "<span class='text-danger'>*</span>" +
            "</label>" +
            "<select class='form-control fieldName' id='fieldName' data-live-search='true'>" +
            "<option value='' selected disabled>Select field name</option>" +
            "<option value='FirstName'> First name</option>" +
            "<option value='SecondName'> Second Name</option>" +
            "<option value='LastName'> Last Name</option>" +
            "<option value='MobileNumber'> Mobile No</option>" +
            "<option value='EmailAddress'> Email Address</option>" +
            "<option value='PhysicalAddress'> Pysical Address </option>" +
            "<option value='Nationality'> Nationality</option>" +
            "<option value='IDNumber'> ID Number </option>" +
            "<option value='SocialSecurityNumber'> Social Sec. Number </option>" +
            "<option value='TerminationDate'> Termination Date </option>" +
            "<option value='MaritalStatus'> Marital Status </option>" +
            "</select>" +
            "</td>" +
            "<td>" +
            "<label for='currentValue' class='block'>Current Value" +
            "</label>" +
            "<input id='currentValue' type='text' class='form-control currentValue'" +
            " style='text-transform: uppercase'" +
            " onchange='alphanumeric(this)' readonly/>" +
            "</td>" +
            "<td class='newValueTd'>" +
            "<label for='newValue' class='block'>New Value" +
            "<span class='text-danger'>*</span>" +
            "</label>" +
            "<input id='newValue' type='text' class='form-control newValue'" +
            " placeholder='Enter value' style='text-transform: uppercase'" +
            " maxlength='50' onchange='alphanumeric(this)' required/>" +
            "</td>" +
            "<td>" +
            "<button class='btn btn-danger btn-sm removeBtn' style='margin-top: 30px'><i class='material-icons' title='Remove'>close</i>Remove</button>" +
            "</td>" +
            "</tr>");
    });

    $(document).on('click', '.removeBtn', function () {
        let currentRow = $(this).closest('tr');
        currentRow.remove();
    });

    $(document).on('change','.fieldName', function () {

        if ($(this).val() !== '') {

            let fieldName = $.trim($(this).val());
            initialiseDatePickerByClass('newValue');
           
            if (fieldName !== 'TerminationDate') {
                $('.newValue').datetimepicker('destroy');
            }
            if (fieldName === 'EmailAddress') {
                $(this).closest('tr').find(".newValueTd").html("<input title='email' id='newValue' class='form-control newValue' style='margin-top: 28px' placeholder='enter new value'>");
           }
            let currentValue = getCurrentValue(fieldName);
            let currentRow = $(this).closest("tr");
            currentRow.find('.currentValue').val(currentValue);
            currentRow.find('.newValue').val('');
        }
    });

    let getCurrentValue = function (fieldName) {

        let currentValue;

        if (fieldName === null || fieldName === '') {
            return '';
        }
        switch (fieldName) {

            case "FirstName":
                currentValue = $.trim($('.firstName').val());
                break;

            case "SecondName":
                currentValue = $.trim($('.secondName').val());
                break;

            case "LastName":
                currentValue = $.trim($('.lastName').val());
                break;

            case "MobileNumber":
                currentValue = $.trim($('.cellPhoneNumber').val());
                break;

            case "EmailAddress":
                currentValue = $.trim($('.emailAddress').val());
                break;

            case "PhysicalAddress":
                currentValue = $.trim($('.physicalAddress').val());
                break;

            case "Nationality":
                currentValue = $.trim($('.nationality').val());
                break;

            case "IDNumber":
                currentValue = $.trim($('.idNumber').val());
                break;

            case "SocialSecurityNumber":
                currentValue = $.trim($('.socialSecurityNumber').val());
                break;

            case "TerminationDate":
                currentValue = $.trim($('.terminationDate').val());
                break;

            case "MaritalStatus":
                currentValue = $.trim($('.maritalStatus').val());
                break;
            default:
                currentValue = "";
        }
        return currentValue;
    }

})

let submitAmendedData = function () {

    let fieldName = [];
    let currentValue = [];
    let newValue = [];
    let employeeCode = $.trim($('#employeeCode').text());
    $('#employeeAmendmentModal').modal('hide');

    if (employeeCode === null || employeeCode === '') {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Employee Code not provided'
        }).then(function () {
            $('#employeeAmendmentModal').modal('show');
        });
        return
    }

    $('.fieldsAmendmentTable tr.fieldsTableRow').each(function () {

        let field = $.trim($(this).closest('tr').find('td:eq(0)').find(':input').val());
        let oldVal = $.trim($(this).closest('tr').find('td:eq(1)').find(':input').val());
        let newVal = $.trim($(this).closest('tr').find('td:eq(2)').find(':input').val());

        if (field !== '' && newVal !== '') {

            if (field === 'TerminationDate') {
                newVal = new Date(newVal).toDateString();
            }
            fieldName.push(field);
            currentValue.push(oldVal)
            newValue.push(newVal)
        }
    });

    if (fieldName.length === 0 || newValue.length === 0) {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Please ensure that the fields and new values are provided'
        }).then(function () {
            $('#employeeAmendmentModal').modal('show');
        });
    } else {

        let payload = {
            "EmployeeCode": employeeCode,
            "FieldName": fieldName,
            "CurrentValue": currentValue,
            "NewValue": newValue,
        };

        swal({
            title: 'Are you sure you want to amend the selected field(s)?',
            text: '',
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmationBtn',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: "Yes",
            closeOnConfirm: true,
        });
        $('.confirmationBtn').click(function () {
            amendmentMethods.submitAmendment(payload);
        });
    }
}