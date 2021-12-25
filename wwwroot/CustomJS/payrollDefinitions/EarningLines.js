$(function () {

    $('.button').click(function () {

        if ($(this).val() === 'Add') {
            $('.modalTitle').text('Add Earning Line');
            $('.EditModalBtn').css('display', "none");
            $('.SubmitBtn').css('display', "block");
            $('.Code').val('');
            $('.Description').val('');
            $('.Status').css('display', 'none');
            $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
        }
        if ($(this).val() === 'Edit') {
            let currentRow = $(this).closest('tr');
            let id = Number(currentRow.find('td:eq(0)').text());
            let code = $.trim(currentRow.find('td:eq(1)').text());
            let desc = $.trim(currentRow.find('td:eq(2)').text());
            let formula = $.trim(currentRow.find('td:eq(3)').text());
            
            code === 'BASIC' ? $('.formula').val(formula).prop('readonly', true): $('.formula').val(formula).prop('readonly', false);
            $('.Code').val(code).prop('readonly', true);
            $('.Description').val(desc);
            $('.LineId').text(id);
            $('.modalTitle').text('Edit Earning Line');
            $('.Status').css('display', 'block');
            $('.SubmitBtn').css('display', 'none');
            $('.EditModalBtn').css('display', 'block');
            $('#AddEditModal').modal('show');
        }
    })

    $('.SubmitBtn').click(function () {

        let code = $.trim($('.Code').val());
        let desc = $.trim($('.Description').val());
        let formula = $.trim($('.formula').val());

        if (code !== '' && desc !== '') {

            $('.btn').prop('disabled', true);
            let model = {
                EarningLineCode: code,
                EarningLineDescription: desc,
                Formula: formula
            };
            $.post('/PayslipDefinition/CreateEarningLine', {model: model})
                .done(function (response) {
                    
                    if(!response.success){
                        $('.btn').prop('disabled', false);
                        swal({title: 'ERROR', text: response.message, type: 'error'});
                        return;
                    }
                    swAlert(response);
                })
                .fail(function (error) {
                    console.log(error);
                });
        } else {
            alertMessage("Please complete all mandatory fields");
        }
    });

    $('.EditModalBtn').click(function () {
        let lineCode = $.trim($('.Code').val());
        let desc = $.trim($('.Description').val());
        let status = $.trim($('#Status').val());
        let formula = $.trim($('.formula').val());

        if (lineCode === '' || status === '0' || desc === '') {
            alertMessage('Please Fill in both fields');
        } else {
            swal({
                title: 'System Response',
                text: 'Are you sure you want to edit this record?',
                type: 'info',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmEditBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $('.confirmEditBtn').click(function () {
                SubmitEditedRecord(lineCode, desc, status, formula);
            });
        }
    });

    function SubmitEditedRecord(lineCode, desc, status, formula) {
        $('.btn').prop('disabled', true);
        $.post("/PayslipDefinition/EditEarningLine", {status: status, definitionCode: lineCode, description: desc, formula: formula})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
            });
    }

    function alertMessage(msg) {
        swal({
            title: 'ERROR',
            text: msg,
            type: 'error',
            buttonsStyling: false,
            confirmButtonClass: 'btn btn-info okBtn'
        });
    }

})