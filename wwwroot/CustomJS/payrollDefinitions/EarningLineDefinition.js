
let customMethods = {

    systemResponse : function(msg) {
        swal({
            title: 'ERROR',
            text: msg,
            type: 'error',
            buttonsStyling: false,
            confirmButtonClass: 'btn btn-info okBtn',
            allowOutsideClick: false
        });
},
    
    submitEarningDefinition: function (dataObject) {
       
        $('#processingProgress').css('display', 'block');
        $('.btn').prop('disabled', true);

        $.post('/PayslipDefinition/CreatePayslipDefinitionDetail', {model: dataObject})
            .done(function (data) {
                $(`#processingProgress`).css('display', 'none');
                swAlert(data);
            })
            .fail(function (error) {
                $('#processingProgress').css('display', 'none');
                customMethods.systemResponse(error.responseText);
            });
},

    GetEarningLines:function () {

        $.post("/PayslipDefinition/EarningLines", "json")
            .done(function (data) {

                if (data.length !== 0) {
                    $('.tbodyEarningLines').empty();

                    $.each(data, function (i, item) {
                        if (item.earningLineCode !== 'BASIC') {
                            $('.tbodyEarningLines').append(`<tr class='earningstblRow'>
                             <td class='code'>${item.earningLineCode}</td><td>${item.earningLineDescription}</td>
                             <td><select class='form-control earningType' name='earningType' id='earningType'>
                             <option value='' selected disabled>Select Type</option>
                             <option value='Percentage'>Percentage </option>
                             <option value='FixedAmount'>Fixed Amount</option>
                             </select></td>
                             <td><select class='form-control frequency' name='frequency' id='frequency' >
                             <option value='' selected disabled>Select Frequency</option>
                             <option value='REC'>Recurring </option>
                             <option value='ONCEOFF'>Once Off </option></td>
                             <td><input type ='number' class='form-control earningValue'></td>
                             </tr>`);
                        }
                    });
                }
            })
            .fail(function (error) {
                console.log(error.responseText);
                customMethods.systemResponse(error.responseText);
            });
}
}

$(document).ready(function () {
    
    customMethods.GetEarningLines();

    $(document).on('change','.earningValue',function () {
        
        let currentRow = $(this).closest('tr');
        let type = currentRow.find('td:eq(2)').find(':input').val();
        let value = $(this).val();

        if ((type === null || type === '')) {
            $(this).val('');
            customMethods.systemResponse('Please select type first');
            return;
        }
        if ((type === "Percentage") && (value <= 0 || value > 100)) {
            $(this).val('');
            customMethods.systemResponse("Percentage value should be between 1 and 100");
            return;
        }
        if (type === "FixedAmount" && (value <= 5)) {
            $(this).val('');
            customMethods.systemResponse("Fixed amount value should be above 5");
        }
    });

    $('.SubmitButton').on('click',function () {

        let earningLineCode = [];
        let description = [];
        let earningType = [];
        let occurenceFrequency = [];
        let earningValue = [];
        let dataObj = {};
        let category = 'Earnings';

        $('#earningsDatatable tr.earningstblRow').each(function () {

            let code = $.trim($(this).closest('tr').find('td:eq(0)').text());
            let desc = $.trim($(this).closest('tr').find('td:eq(1)').text());
            let type = $.trim($(this).closest('tr').find('td:eq(2)').find(':input').val());
            let frequency = $.trim($(this).closest('tr').find('td:eq(3)').find(':input').val());
            let value = Number($.trim($(this).closest('tr').find('td:eq(4)').find(':input').val()));

            if (type !== '' && value !== 0 && desc !=='' && frequency !=='') {
                earningLineCode.push(code);
                description.push(desc);
                earningType.push(type);
                occurenceFrequency.push(frequency);
                earningValue.push(value);
            }
        });

        if (earningType.length === 0) {
            customMethods.systemResponse('Please provide earning type(s)');
            return;
        }
        if (occurenceFrequency.length === 0) {
            customMethods.systemResponse('Please provide occurrence frequency');
            return;
        }
        if (earningValue.length === 0) {
            customMethods.systemResponse('Please provide earning values');
        } else {
            dataObj = {
                'Code': earningLineCode,
                'Description': description,
                'Type': earningType,
                'Value': earningValue,
                'OccurenceCode': occurenceFrequency,
                'Category': category,
                'EmployeeCode': $.trim($('.employeeName').text())
            };
            customMethods.submitEarningDefinition(dataObj);
        }
    });
    
})