
let $totalEarnings = $('#totalEarnings');
let $totalDeductions = $('#totalDeductions');

let payslipDetailsMethods = {

    getPaySlipDetails: function (employeeCode) {

        $.post('/PayslipDetails/EmployeeEarningsAndDeductions', {employeeCode: employeeCode})
            .done(function (response) {
                if (!response.success) {
                    swal({
                        title: 'Payslip details not found',
                        text: response.message,
                        type: "error",
                        buttonsStyling: false,
                        confirmButtonClass: "btn btn-info okBtn",
                        allowOutsideClick: false
                    }).then(function (){
                        $totalEarnings.text('');
                        $totalDeductions.text('');
                        $('.earningsTbody').empty();
                        $('.deductionsTbody').empty();
                    });
                } else {
                    swal({
                        title: 'Payslip details fetched successfully',
                        text: 'Click ok to proceed',
                        type: 'success',
                        buttonsStyling: false,
                        confirmButtonClass: "btn btn-success okBtn",
                        allowOutsideClick: false
                    }).then(function () {
                        payslipDetailsMethods.showDEarningsAndDeductions(response);
                    });
                }

            })
            .fail(function (error) {
                console.log(error);
            });
    },
    
    showDEarningsAndDeductions: function (response){

        $('.earningsTbody').empty();
        $('.deductionsTbody').empty();
        $totalEarnings.text('');
        $totalDeductions.text('');
        let totalEarnings = 0.0;
        let totalDeductions = 0.0;

        $.each(response.payload, function (i, item) {

            let flagId = item.payrollDefinitionFlag;

            if (flagId === 1) {
                $('.earningsTbody').append(`<tr class="earningsTableRow col-lg-12">
                <td class="code"> ${item.payrollDefinitionCode} </td>
                <td> ${item.description} </td>
                <td> ${item.type} </td >
                <td class="value"><input type ='number' value="${item.value}" class="form-control" readonly></td>
                <td class="amount"><input type ='number' value="${item.amount}" class="form-control" readonly></td>
                 <td class="text-right">
                 <button type="button" rel="tooltip" class="btn btn-info btn-link btn-sm amendmentButton" data-original-title="" title="Amend"> <i class="material-icons">edit</i></button>
                 <button type="button" rel="tooltip" class="btn btn-success btn-link btn-sm submitAmendmentButton" data-original-title="" title="Amend" style="display: none"> Submit </button>        
                 </td>
                 <td>
                      <button type="button" rel="tooltip" class="btn btn-danger btn-link btn-sm removeBtn" data-original-title="remove" title="">
                       <i class="material-icons">close</i>
                       <div class="ripple-container"></div></button>
                 </td>
                 <td class="lineType d-none">Earnings</td>
                </tr>`);

                totalEarnings += item.amount;
            }
            if (flagId === 2) {
                $('.deductionsTbody').append(`<tr class="deductionTableRow col-lg-12">
                <td class="code"> ${item.payrollDefinitionCode} </td>
                <td> ${item.description} </td>
                <td> ${item.type} </td>
                <td class="value"><input type ='number' value="${item.value}" class="form-control" readonly></td>
                <td><input type ='number' value="${item.amount}" class="form-control" readonly></td>
                 <td class="text-right">
                 <button type="button" rel="tooltip" class="btn btn-info btn-link btn-sm amendmentButton" data-original-title="" title="Amend"> <i class="material-icons">edit</i></button>
                 <button type="button" rel="tooltip" class="btn btn-success btn-link btn-sm submitAmendmentButton" data-original-title="" title="Amend" style="display: none"> Submit </button>        
                 </td>
                 <td>
                      <button type="button" rel="tooltip" class="btn btn-danger btn-link btn-sm removeBtn" data-original-title="remove" title="">
                       <i class="material-icons">close</i>
                       <div class="ripple-container"></div></button>
                 </td>
                 <td class="lineType d-none">Deductions</td>
                </tr>`);

                totalDeductions += item.amount;
            }
        });
        $('.earningsAndDeductionsDiv').css('display', 'block');
        $totalEarnings.text(this.formatNumber(totalEarnings));
        $totalDeductions.text(this.formatNumber(totalDeductions));
        $('#netPay').text(this.formatNumber(totalEarnings - totalDeductions));
        
    },

    submitAmendment : function (employeeCode, code, amendedValue, payrollDefType){
    
        $('.btn').prop("disabled", true);
        let amendmentModel = {
            EmployeeCode: employeeCode,
            Code: code,
            AmendedValue: amendedValue,
            PayrollDefinitionType: payrollDefType
        };
        $.post(`/PayslipDetails/EarningsAndDeductionsAmendment`, {request: amendmentModel})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                $('.btn').prop("disabled", false);
                console.log(error);
                payslipDetailsMethods.response(error.responseText);
            });
    },
 
    removePayrollLine : function (employeeCode, lineCode){
        $('.btn').prop("disabled", true);
        
        let payload = {
            EmployeeCode: employeeCode,
            Code: lineCode,
            AmendedValue: 1,
            PayrollDefinitionType: 'X'
        };
        $.post(`/PayslipDetails/RemoveEmployeePayrollLine`, {request: payload})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                $('.btn').prop("disabled", false);
                console.log(error);
                payslipDetailsMethods.response(error.responseText);
            });
    },
    
    formatNumber : function (value){
        return Intl.NumberFormat('en-US').format(value.toFixed(2));
    },
    
    response: function (msg) {
        swal({
            title: 'Error',
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn",
            allowOutsideClick: false
        });
    },
    
}

$(function () {
    
    $('.payslipDetailsBtn').on('click', function () {

        let employee = $('#search').val();

        if (employee === '') {
            swal({
                title: 'Error',
                text: 'Please provide the employee code ',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn",
                allowOutsideClick: false
            });
        } else {
            let employeeCode = employee.split(' ')[2];
            payslipDetailsMethods.getPaySlipDetails(employeeCode);
        }
    });
    
    $(document).on('click','.amendmentButton', function (){
        
        let currentRow = $(this).closest('tr');
        let code = $.trim(currentRow.find('.code').text());
        let actionButton = currentRow.find('td:eq(5)').find('.amendmentButton');
        let submitButton = currentRow.find('td:eq(5)').find('.submitAmendmentButton');
        
        if(code === 'BASIC' || code === 'PAYE' || code ==='NAPSA' || code ==='NHIMA'){
            swal({
                title: 'Amendment of ' + code +' is currently not allowed',
                type: 'warning',
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn",
                allowOutsideClick: false
            });
        }
        else {
            currentRow.find('td:eq(3)').find(':input').prop('readonly',false);
            actionButton.css('display','none');
            submitButton.css('display','block'); 
        }
    });

    $(document).on('change','.value',function () {
        
        let currentRow = $(this).closest('tr');
        let type = $.trim(currentRow.find('td:eq(2)').text());
        let submitButton = currentRow.find('td:eq(5)').find('.submitAmendmentButton');
        let actionButton = currentRow.find('td:eq(5)').find('.amendmentButton');
        let value = Number(currentRow.find('.value').find(':input').val());

        if ((type === "Percentage") && (value <= 0 || value > 100)) {
            submitButton.css('display','none');
            actionButton.css('display','block')
            currentRow.find('.value').find(':input').val('');
            payslipDetailsMethods.response('Percentage value should be between 1 and 100');
            return;
        }
        if (type === "FixedAmount" && (value <= 5)) {
            submitButton.css('display','none');
            actionButton.css('display','block');
            payslipDetailsMethods.response("Fixed amount value should be above 5");
        }
    })
    
    $(document).on('click','.submitAmendmentButton',function (){
        
        let currentRow = $(this).closest('tr');
        let amendedValue = Number(currentRow.find('.value').find(':input').val());
        let code =  $.trim(currentRow.find('.code').text());
        let employeeCode = $('.employeeCode').val().split(' ')[2];
        let payrollDefinitionType = $.trim(currentRow.find('.lineType').text());
        
        if(amendedValue === 0){
            payslipDetailsMethods.response('Value can not be null');  
            return;
        }
        swal({
            title: 'Are you sure?',
            text: 'Are you sure you want to amend this value?',
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmBtn',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: "Yes",
            buttonsStyling: false,
            allowOutsideClick: false,
        });
        $('.confirmBtn').click(function () {
            payslipDetailsMethods.submitAmendment(employeeCode, code, amendedValue, payrollDefinitionType)
        });
    })

    $(document).on('click','.removeBtn',function (){

        let currentRow = $(this).closest('tr');
        let code =  $.trim(currentRow.find('.code').text());
        let employeeCode = $('.employeeCode').val().split(' ')[2];

        swal({
            title: 'Are you sure?',
            text: 'Are you sure you want to remove this line?',
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmBtn',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: "Yes",
            buttonsStyling: false,
            allowOutsideClick: false,
        });
        $('.confirmBtn').click(function () {
            payslipDetailsMethods.removePayrollLine(employeeCode, code)
        });
    })
})

