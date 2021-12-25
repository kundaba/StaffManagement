$(document).ready(function () {
    
    demo.initDateTimePicker();

    $(".button").click(function () {

        if ($(this).val() === "Add") {
            $(".modalTitle").text("Add Tax Definition Detail");
            $(".EditBtn").css("display", "none");
            $(".Status").css("display", "none");
            $(".SubmitBtn").css("display", "block");
            $(".form-control").val("");
            $("#AddEditTaxDefModal").modal("show");
            $('#AddEditTaxDefModal').modal({ backdrop: 'static', keyboard: false });
        }
        if ($(this).val() === "Edit") {
            let currentRow = $(this).closest('tr');
            let id = Number(currentRow.find('td:eq(0)').text());
            let description = $.trim(currentRow.find('td:eq(1)').text());
            let lowerLimit = Number(currentRow.find('td:eq(2)').text());
            let upperLimit = Number(currentRow.find('td:eq(3)').text());
            let amount = Number(currentRow.find('td:eq(4)').text());
            let percentage = Number(currentRow.find('td:eq(5)').text());
            $(".Id").text(id);
            $(".description").val(description).prop("readonly", true);
            $(".LowerLimit").val(lowerLimit);
            $(".UpperLimit").val(upperLimit);
            $(".Amount").val(amount);
            $(".Percentage").val(percentage);
            $(".modalTitle").text("Edit Tax Definition Detail");
            $(".SubmitBtn").css("display", "none");
            $(".EditBtn").css("display", "block");
            $("#Status").css("display", "block");
            $("#AddEditTaxDefModal").modal("show");
            $('#AddEditTaxDefModal').modal({ backdrop: 'static', keyboard: false });
        }
    })

    $(".SubmitBtn").click(function () {

        let description = $.trim($(".description").val());
        let lowerLimit = $(".LowerLimit").val();
        let upperLimit = $(".UpperLimit").val();
        let amount =     $(".Amount").val();
        let percentage = $(".Percentage").val();

        if (description !== "" && (lowerLimit !== "")) {

            $(".btn").prop("disabled", true);

            let model = {
                BandDescription: description,
                LowerLimit: lowerLimit,
                UperLimit: upperLimit,
                Amount: amount,
                Percentage: percentage,
            };
            $.post("/Parameters/AddEditTaxDefinition", { model: model, task:"Add" })
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    alertMessage(error.responseText);
                });
        }
        else {
            alertMessage("Please Fill at least description and lower limit");
        }

    });
    $(".EditBtn").click(function () {

        let id = Number($(".Id").text());
        let description = $.trim($(".description").val());
        let lowerLimit = $(".LowerLimit").val();
        let upperLimit = $(".UpperLimit").val();
        let amount = $(".Amount").val();
        let percentage = $(".Percentage").val();
        let status = $("#Status").val();

        if (status !== "" && description !=="") {

            let editedModel = {
                Id: id,
                BandDescription: description,
                LowerLimit: lowerLimit,
                UperLimit: upperLimit,
                Amount: amount,
                Percentage: percentage,
                Status: status
            };
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
            $(".confirmEditBtn").click(function () {
                SubmitEditedRecord(editedModel)
            });
        }
        else {
            alertMessage("Please Fill in all fields");
        }
    });

    function SubmitEditedRecord(editedModel) {
        $(".btn").prop("disabled", true);

        $.post("/Parameters/AddEditTaxDefinition", { model: editedModel,task:"Edit" })
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
            type: "info",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn"
        }).then(function () {
            window.location.reload(true);
        });
    }
})