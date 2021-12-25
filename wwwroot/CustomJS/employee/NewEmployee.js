
let methods = {

    addNewEmployee : function(data, progressBar) {
        progressBar.css("display", "block");
        $(".btn").prop("disabled", true);
        
        $.post("/Employee/AddNewEmployee", { employee: data }, "json")
            .done(function (response) {
                progressBar.css("display", "none");
                console.log(response);
                let title = '';
                let alertType = ''

                if (response.status === 200)
                {
                    title =  'Employee Added';
                    alertType = 'success';
                }
                if (response.status === 500)
                {
                    title =  'Error';
                    alertType = 'error';
                    swal({title: title, type: alertType, text: response.message});
                    return;
                }
                swal({
                    title: title,
                    text: response.message,
                    type: alertType,
                    confirmButtonClass: 'btn btn-info',
                    confirmButtonText: "Ok",
                    closeOnConfirm: true
                }).then(function () {
                    $('#employeeCode').val("");
                    window.location.href="/Employee/Index";
                });
            })
            .fail(function (error) {
                progressBar.css("display", "none");
                console.log(error.responseText);
                methods.systemResponse(error.responseText);
            });
    },

    calculateEmployeeAge : function(dateString) {
        let today = new Date();
        let birthDate = new Date(dateString);
        let age = today.getFullYear() - birthDate.getFullYear();
        let m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    },

    systemResponse : function (msg, txtFiled) {
        swal({
            title: "error",
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn",
            allowOutsideClick: false
        }).then(function () {
            $("#" + txtFiled).val("");
        });
    },

    validateInputData : function(inputData, dataType) {
        let regexValues = "";
        if (dataType === "Text") {
            regexValues = /^[A-Za-z ]+$/;
        }
        if (dataType === "Numeric") {
            regexValues = /^[0-9]+$/;
        }
        if (dataType === "AlphaNumeric") {
            regexValues = /^[A-Za-z0-9 ]+$/;
        }
        if ($('#' + inputData).val().match(regexValues)) {
            return true;
        }
        else {
            $("#" + inputData).val("");
            return false;
        }
    },

    validateNumericValues(value,textField){

        if(value < 50 || value > 100000){
            $("."+textField).val("");

            swal({
                title: 'Error',
                text: 'Please enter a valid value for between 50 and 100, 000',
                type: 'error',
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info"
            }).then(function () {
                $('#AddRecordModal').modal('show');
            });

        }
    },
    
    getFormData : function (){
        return {
            "EmployeeCode": $("#employeeCode").val(),
            "DateEngaged": $("#engagementDate").val(),
            "TerminationDate ": $("#terminationDate").val(),
            "JobTitleId": $("#jobTitle").val(),
            "JobGradeId": $("#grade").val(),
            "JobGeneralId": $("#jobGeneral").val(),
            "NatureOfContractId": $("#contractType").val(),
            "DepartmentId": $("#department").val(),
            "FirstName": $("#firstname").val(),
            "SecondName": $("#secondname").val(),
            "LastName": $("#lastname").val(),
            "BirthDate": $("#dob").val(),
            "Gender": $("#gender").val(),
            "Nationality": $("#nationality").val(),
            "MaritalStatusId": $("#maritalStatus").val(),
            "IdNumber": $("#employeeId").val(),
            "IdNumberType": $("#IDType").val(),
            "SocialSecurityNumber": $("#ssn").val(),
            "WorkNumber": $("#workNumber").val(),
            "CellNumber": $("#cellNumber").val(),
            "EmailAddress": $("#emailAddress").val(),
            "PhysicalAddress": $("#physicalAddress").val(),
            "BankId": $("#bank").val(),
            "BankBranchId": $("#branch").val(),
            "AccountName": $("#accountName").val(),
            "AccountNumber": $("#accountNumber").val(),
            "BasicPay": Number($("#basicPay").val()),
        };
    }

};

$(function () {

    demo.initMaterialWizard();
    
    setTimeout(function () {
        $('.card.card-wizard').addClass('active');
    }, 400);

    $('#firstname').keyup(function () {
        methods.validateInputData("firstname", "Text");
    });
    
    $('#secondname').keyup(function () {
        methods.validateInputData('secondname', "Text");
    });
    $(`#lastname`).keyup(function () {
        methods.validateInputData("lastname", "Text");
    });
    
    $('#employeeId').keyup(function () {
        methods.validateInputData("employeeId", "AlphaNumeric");
    });
    
    $('#physicalAddress').keyup(function () {
        methods.validateInputData("physicalAddress", "AlphaNumeric");
    });
    
    $('#cellNumber').keyup(function () {
        methods.validateInputData("cellNumber", "Numeric");
    });
    
    $('#ssn').keyup(function () {
        methods.validateInputData("ssn", "Numeric");
    });
    
    $('#accountName').keyup(function () {
        methods.validateInputData("accountName", "Text");
    });
    
    $('#accountNumber').keyup(function () {
        methods.validateInputData("accountNumber", "Numeric");
    }); 
    
    $('#basicPay').on('change', function () {
        methods.validateNumericValues($(this).val(), "basicPay");
    });

    $('.submitButton').on('click', function () {
        
        if (!isValidForm()) {
            swal({
                title: "error",
                text: 'Please provide all the mandatory information',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn",
                allowOutsideClick: false
            });
        } else {
            
            swal({
                title: 'Are you sure you want to engage this employee?',
                text: 'Note that this action is irreversible',
                type: 'info',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $('.confirmBtn').click(function () {
                let progressBar = $('#processingProgress');
                $(this).prop('disabled', true);
                let formData = methods.getFormData();
                methods.addNewEmployee(formData,progressBar);
            });
        }
    });

    function isValidForm() {
        let result = false;
        let employeeCode = $("#employeeCode").val();
        let accountName = $("#accountName").val();
        let accountNumber = $("#accountNumber").val();
        let bank = $("#bank").val();
        let branch = $("#branch").val();
        let basicPay = $("#basicPay").val();

        if (employeeCode !== "" && accountName !== "" && accountNumber !== "" && bank !== "" && branch !== "" && basicPay !=="") {
            result = true;
        }
        return result;
    }

    $('#dob').on('blur',function () {
        let dob = $(this).val();
        let employeeAge = Number(methods.calculateEmployeeAge(dob));
        if (employeeAge < 16) {
            methods.systemResponse("Employee should be 16 yrs or more", "dob");
            $(this).css("border", "solid 1px red");
        }
        else {
            $(this).css("border", "solid 1px green");
            return true;
        }
    });

    $('#lastname').on('blur', function () {
        let fName = $("#firstname").val();
        let lName = $("#lastname").val();
        if (fName !== "" && lName !== "") {
            $("#accountName").val(fName + " " + lName);
        }
    })

})

let validateNrc = function (nrcNo){

    let lastDigit = nrcNo.charAt(nrcNo.length - 1);

    if (nrcNo < 0 || nrcNo.length !== 9) {
        $('#employeeId').val('');
        swal({title: 'Invalid NRC Number', type: 'error', text: 'NRC number should contain 9 digits'});
        return false;
    }

    if (lastDigit !== '1' && lastDigit !== '2') {
        $('#employeeId').val('');
        swal({title: 'Invalid NRC Number', type: 'error', text: 'NRC number should end with 1 or 2'});
        return false;
    }
}

let validateAccountNumber = function (accountNo){
    
    if(accountNo === '' || accountNo <= 0 || accountNo.length !== 13){
        $('#accountNumber').val('');
        swal({title:'Invalid Account Number', type: 'error', text: 'Account number should contain 13 digits'});
        return false;
    }
}