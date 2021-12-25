$(document).ready(function () {

    $(".button").click(function () {
        if ($(this).val() === "Add") {
            $(".modalTitle").text("Add JobGrade");
            $(".EditModalBtn").css("display", "none");
            $(".SubmitBtn").css("display", "block");
            $(".JobGradeCode").val("");
            $(".LongDescription").val("");
            $(".Status").css("display", "none");
            $("#AddEditModal").modal({backdrop: 'static', keyboard: false});
        }
        if ($(this).val() === "Edit") {
            let currentRow = $(this).closest('tr');
            let id = Number(currentRow.find('td:eq(0)').text());
            let jobGradeCode = $.trim(currentRow.find('td:eq(1)').text());
            let longDescription = $.trim(currentRow.find('td:eq(2)').text());
            $(".JobGradeCode").val(jobGradeCode).prop("readonly", true);
            $(".LongDescription").val(longDescription);
            $(".JobGradeId").text(id);
            $(".modalTitle").text("Edit JobGrade");
            $(".Status").css("display", "block");
            $(".SubmitBtn").css("display", "none");
            $(".EditModalBtn").css("display", "block");
            $("#AddEditModal").modal({backdrop: 'static', keyboard: false});
        }
    })

    $(".SubmitBtn").click(function () {

        let jobGradeCode = $(".JobGradeCode").val();
        let longDesc = $(".LongDescription").val();

        if (jobGradeCode !== "" && longDesc !== "") {

            $(".btn").prop("disabled", true);

            let model = {
                JobGradeCode: jobGradeCode,
                JobGradeDescription: longDesc
            };

            $.post("/Parameters/AddJobGrade", { model: model })
                .done(function (response) {
                    swAlert(response);
                })
                .fail(function (error) {
                    alertMessage(error.responseText);
                });
        }
        else {
            alertMessage("Please Fill in both fields");
        }
    });

    $(".EditModalBtn").click(function () {

        let id = Number($(".JobGradeId").text());
        let longDesc = $.trim($(".LongDescription").val());
        let status = $.trim($("#Status").val());

        if (id !== 0 && longDesc !== "" && status !== "0") {
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
                SubmitEditedRecord(id, longDesc, status);
            });
        }
        else {
            alertMessage("Please Fill in both fields");
        }
    });

    function SubmitEditedRecord(id, longDescription, status) {
        $(".btn").prop("disabled", true);

        $.post("/Parameters/EditJobGrade", { gradeId: id, description: longDescription, status: status })
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