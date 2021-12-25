
let deductionDefinitionMethods = {
    
    GetDeductionLines : function() {
        $.post('/PayslipDefinition/DeductionLines', 'json')
            .done(function (data) {
                if (data !== null) {
                    $(".tbodyDeductionLines").empty();
                    $.each(data, function (i, item) {
                        
                        let code = item.deductionCode;

                        if (code !== 'PAYE' && code !== 'NAPSA' && code !=='NHIMA') {
                            $('.tbodyDeductionLines').append(`<tr class="deductionstblRow">
                                <td>${item.deductionCode}</td>
                                <td>${item.deductionDescription}</td>
                                <td><select class='form-control deductionType'  id='deductionType'>
                                <option value='' selected disabled>Select Type</option>
                                <option value='Percentage'>Percentage </option>
                                <option value='FixedAmount'>Fixed Amount</option>
                                </select></td>
                                <td><select class='form-control frequency' name='frequency' id='frequency' >
                                <option value='' selected disabled>Select Frequency</option>
                                <option value='REC'>Recurring </option>
                                <option value='ONCEOFF'>Once Off </option>
                                </select></td>
                               <td><input type ='number' class='form-control deductionValue' name='deductionValue'></td>
                               </tr>`);
                        }
                    });
                }
            })
            .fail(function (error) {
                console.log(error);
            });
},

    submitDeductionDefinition: function (dataObject) {
        $('#processingProgress').css('display', 'block');
        $('.btn').prop('disabled', true);

        $.post('/PayslipDefinition/CreatePayslipDefinitionDetail', {model: dataObject})
            .done(function (response) {
                $('#processingProgress').css('display', 'none');
                
                if(!response.success){
                    $('.btn').prop('disabled', false);
                    swal({title: 'ERROR', text: response.message, type: 'error'});
                    return;
                }
                swAlert(data);
            })
            .fail(function (error) {
                console.log(error);
                $('#processingProgress').css('display', 'none');
                deductionDefinitionMethods.systemResponse(error.responseText);
            });
},

    systemResponse: function (msg) {
        swal({
            title: 'Error',
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn",
            allowOutsideClick: false
        });
    }
}


$(document).ready(function () {
    deductionDefinitionMethods.GetDeductionLines();
    
    $(document).on('change','.deductionValue',function () {
        let currentRow = $(this).closest('tr');
        let type = currentRow.find('td:eq(2)').find(':input').val();
        let value = $(this).val();

        if ((type === null || type === '')) {
            $(this).val('');
            deductionDefinitionMethods.systemResponse('Please select type first');
            return;
        }
        if ((type === "Percentage") && (value <= 0 || value > 100)) {
            $(this).val('');
            deductionDefinitionMethods.systemResponse("Percentage value should be between 1 and 100");
            return;
        }
        if (type === "FixedAmount" && (value <= 5)) {
            $(this).val('');
            deductionDefinitionMethods.systemResponse("Fixed amount value should be above 5");
        }
    });

    $('.SubmitBtn').click(function () {

        let deductionLineCode = [];
        let description = [];
        let deductionType = [];
        let deductionValue = [];
        let dataObj = {};
        let category = "Deductions";

        $("#deductionsDatatable tr.deductionstblRow").each(function () {

            let code = $.trim($(this).closest('tr').find('td:eq(0)').text());
            let desc = $.trim($(this).closest('tr').find('td:eq(1)').text());
            let type = $.trim($(this).closest('tr').find('td:eq(2)').find(':input').val());
            let value = Number($.trim($(this).closest('tr').find('td:eq(3)').find(':input').val()));

            if (type !== '' && value !== 0 && desc !== null) {
                deductionLineCode.push(code);
                description.push(desc);
                deductionType.push(type);
                deductionValue.push(value);
            }
        });
        
        if (deductionType.length === 0) {
            customMethods.systemResponse('Please provide deduction type(s)');
            return;
        }
        if (deductionValue.length === 0) {
            customMethods.systemResponse('Please provide deduction values');
        } else {

            dataObj = {
                "Code": deductionLineCode,
                'Description': description,
                "Type": deductionType,
                "Value": deductionValue,
                "Category": category,
                "EmployeeCode": $.trim($('#employeeCode').text())
            };
            console.log(dataObj);
           deductionDefinitionMethods.submitDeductionDefinition(dataObj);
        }
    });
    
})