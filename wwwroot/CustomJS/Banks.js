$(document).ready(function () {

    $('.button').click(function () {

        if ($(this).val() === "Add") {
            $('.modalTitle').text("Add Bank");
            $(".EditModalBtn").css("display", "none");
            $('.SubmitBtn').css("display", "block");
            $('.BankCode').val("");
            $('.BankName').val("");
            $('.Status').css("display", "none");
            $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
        }
        if ($(this).val() === "Edit") {

            let currentRow = $(this).closest('tr');
            let id = Number(currentRow.find('td:eq(0)').text());
            let bankCode = $.trim(currentRow.find('td:eq(1)').text());
            let bankName = $.trim(currentRow.find('td:eq(2)').text());
            $('.BankCode').val(bankCode).prop("readonly", true);
            $('.BankName').val(bankName);
            $('.BankId').text(id);
            $('.modalTitle').text('Edit Bank');
            $('.Status').css('display', "block");
            $('.SubmitBtn').css('display', "none");
            $('.EditModalBtn').css('display', "block");
            $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
        }
    })

    $(".SubmitBtn").click(function () {

        let bankCode = $(".BankCode").val();
        let bankName = $(".BankName").val();

        if (bankCode !== "" && bankName !== "") {

            $(".btn").prop("disabled", true);
            let model = {
                BankCode: bankCode,
                BankName: bankName
            };
            $.post("/Parameters/AddBank", { model: model })
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    alertMessage(error.responseText);
                });
        }
        else {
            alertMessage("Please provide required data");
        }
    });

    $(".EditModalBtn").click(function () {

        let id = Number($(".BankId").text());
        let bankCode = $.trim($(".BankCode").val());
        let bankName = $.trim($(".BankName").val());
        let status = $.trim($("#Status").val());

        if (!(id !== 0 && bankName !== "" && status !== "")) {
            alertMessage("Please Fill in both fields");
        } else {
            swal({
                title: 'Action Confirmation',
                text: 'Are you sure you want to edit this record?',
                type: 'info',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmEditBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $(".confirmEditBtn").click(function () {
                SubmitEditedRecord(id, bankCode, bankName, status);
            });
        }
    });

    function SubmitEditedRecord(id, bankCode, bankName, status) {
        $(".btn").prop("disabled", true);

        let model = {
            BankId: id,
            BankCode: bankCode,
            BankName: bankName,
            Status: status
        };
        $.post("/Parameters/EditBankName", { model: model })
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                alertMessage(error.responseText);
            });
    }

    function alertMessage(msg) {
        swal({
            title: "System Response",
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn",
            allowOutsideClick: false,
        });
    }
})