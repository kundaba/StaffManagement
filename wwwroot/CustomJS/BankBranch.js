$(document).ready(function () {

    $(".button").click(function () {
        if ($(this).val() === "Add") {
            GetBankList();
            $('.modalTitle').text("Add Branch");
            $('.EditModalBtn').css("display", "none");
            $('.SubmitBtn').css("display", "block");
            $('.BranchCode').val("");
            $('.BranchName').val("");
            $('.Status').css("display", "none");
            $('.BankName').css("display", "block");
            $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
        }
        if ($(this).val() === "Edit") {
            let currentRow = $(this).closest('tr');
            let branchCode = $.trim(currentRow.find('td:eq(0)').text());
            let branchName = $.trim(currentRow.find('td:eq(1)').text());
            $('.BranchCode').val(branchCode).prop("readonly", true);
            $('.BranchName').val(branchName);
            $('.modalTitle').text("Edit Branch");
            $('.Status').css("display", "block");
            $('.BankName').css("display", "none");
            $('.SubmitBtn').css("display", "none");
            $('.EditModalBtn').css("display", "block");
            $('#AddEditModal').modal({backdrop: 'static', keyboard: false});
        }
    })

    $('.SubmitBtn').click(function () {

        let branchCode = $('.BranchCode').val();
        let branchName = $('.BranchName').val();
        let bankId = Number($('#BankName').val());

        if (branchCode !== '' && branchName !== '' && bankId !== 0) {

            $('.btn').prop('disabled', true);
            let model = {
                BankId : bankId,
                BranchCode: branchCode,
                BranchName: branchName
            };
            $.post("/Parameters/AddBranch", { model: model })
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    alertMessage(error.responseText);
                });
        }
        else {
            alertMessage("Please Complete all fields");
        }
    });

    $('.EditModalBtn').click(function () {

        let branchCode = $.trim($(".BranchCode").val());
        let branchName = $.trim($(".BranchName").val());
        let status = $.trim($("#Status").val());

        if (!(branchCode !== "" && branchName !== "" && status !== "")) {
            alertMessage("Please Complete all fields");
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
            $('.confirmEditBtn').click(function () {
                SubmitEditedRecord(branchCode, branchName, status);
            });
        }
    });

    function SubmitEditedRecord(branchCode, branchName, status) {
        $('.btn').prop("disabled", true);
        let model = {
            BranchCode: branchCode,
            BranchName: branchName,
            Status: status
        };
        $.post("/Parameters/EditBranch", { model: model })
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                alertMessage(error.responseText);
            });
    }
    function alertMessage(msg) {
        swal({
            title: 'System Response',
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn"
        });
    }

    function GetBankList() {
        $.get('/Parameters/Bank', "json")
            .done(function (data) {
                let bank = $(".BankName");
                if (data.length > 0) {
                    bank.empty();

                    $.each(data, function (i, item) {
                        bank.append("<option value='" + item.bankId + "'>" + item.bankName + "</option>").selectpicker('refresh');
                    });
                }
                bank.prepend("<option selected value=''> Select Bank </option>")
            });
    }

})