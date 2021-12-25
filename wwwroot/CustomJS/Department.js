
let methods = {

    addNewDepartment: function (data) {

        $.post("/Parameters/AddDepartment", {model: data})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                methods.alertMessage(error.responseText);
            });
    },

    confirmDelete: function (deptId) {
       
        $(".btn").prop("disabled", true);
        $.post("/Parameters/DeleteDepartment", {id: deptId})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                methods.alertMessage(error.responseText);
            });
    },

    SubmitEditedRecord: function (deptId, deptCode, deptDescription, status) {
        
        $(".btn").prop("disabled", true);
        let editedModel = {
            DepartmentId: deptId,
            DepartmentCode: deptCode,
            LongDescription: deptDescription,
            Status: status
        };
        $.post("/Parameters/EditDepartment", {model: editedModel})
            .done(function (response) {
                swAlert(response);
            })
            .fail(function (error) {
                console.log(error);
                methods.alertMessage(error.responseText);
            });
    },

    alertMessage: function (msg) {
        swal({
            title: "ERROR",
            text: msg,
            type: "error",
            buttonsStyling: false,
            confirmButtonClass: "btn btn-info okBtn"
        });
    }
    
}


$(function () {
    
    demo.initDateTimePicker();
    
    $(".button").click(function () {

        if ($(this).val() === "Add") {
            $(".modalTitle").text("Add Department");
            $(".EditModalBtn").css("display", "none");
            $(".SubmitBtn").css("display", "block");
            $(".DepartmentCode").val("");
            $(".LongDescription").val("");
            $(".Status").css("display", "none");
            $("#AddEditDeptModal").modal({backdrop: 'static', keyboard: false});
        }
        if ($(this).val() === "Edit") {
            let currentRow = $(this).closest('tr');
            let deptId = Number(currentRow.find('td:eq(0)').text());
            let deptCode = $.trim(currentRow.find('td:eq(1)').text());
            let description = $.trim(currentRow.find('td:eq(2)').text());
            $(".DepartmentCode").val(deptCode).prop("readonly",true);
            $(".LongDescription").val(description);
            $(".DepartmentId").text(deptId);
            $(".modalTitle").text("Edit Department");
            $(".SubmitBtn").css("display", "none");
            $(".Status").css("display", "block");
            $(".EditModalBtn").css("display", "block");
            $("#AddEditDeptModal").modal({backdrop: 'static', keyboard: false});
        }
    })

    $(".SubmitBtn").click(function () {

        let dptCode = $(".DepartmentCode").val();
        let description = $(".LongDescription").val();
        if (!(dptCode !== "" && description !== "")) {
            methods.alertMessage("Please complete all mandatory fields");
        } else {

            $(".btn").prop("disabled", true);
            let data = {
                DepartmentCode: dptCode,
                LongDescription: description,
                Status: 'A'
            };
            
            swal({
                title: 'System Response',
                text: 'Are you sure you want to add this department?',
                type: 'info',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmSubmissionBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $(".confirmSubmissionBtn").click(function () {
                methods.addNewDepartment(data);
            });
        }
    });

    $(".EditModalBtn").click(function () {
        let deptId = Number($(".DepartmentId").text());
        let dptCode = $.trim($(".DepartmentCode").val());
        let description = $.trim($(".LongDescription").val());
        let status = $.trim($("#Status").val());
        
        if (!(deptId !== 0 && dptCode !== "" && description !== "" && status !== "")) {
            methods.alertMessage("Please complete all the mandatory fields");
        } else {
            swal({
                title: 'System Response',
                text: 'Are you sure you want to edit this record?',
                type: 'warning',
                showCancelButton: true,
                confirmButtonClass: 'btn btn-success confirmEditBtn',
                cancelButtonClass: 'btn btn-danger',
                confirmButtonText: "Yes",
                buttonsStyling: false,
                allowOutsideClick: false,
            });
            $(".confirmEditBtn").click(function () {
                methods.SubmitEditedRecord(deptId, dptCode, description, status)
            });
        }
    });

    $(".DeleteBtn").click(function () {

        let deptId = Number($(this).closest('tr').find('td:eq(0)').text());
        swal({
            title: 'System Response',
            text: 'Are you sure you want to make this department inactive?',
            type: 'info',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success confirmDeleteBtn',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: "Delete",
            buttonsStyling: false,
            allowOutsideClick: false,
        });
        $(".confirmDeleteBtn").click(function () {
            methods.confirmDelete(deptId);
        });
    });

})