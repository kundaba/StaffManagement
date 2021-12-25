let methods = {

    getAllDisciplinaryCases: function () {

        $.post("/DisciplinaryCases/GetDisciplinaryCase/")
            .done(function (response) {

                if (!response.success) {
                    swal({title: 'ERROR', type: 'error', text: response.message})
                    return;
                }
                if (response.payload === null || response.payload.length === 0) {
                    swal({title: 'No Data Found', type: 'info', text: 'No disciplinary cases found'})
                    return;
                }
                let data = response.payload;
                let tableBody = $('.dTBody');
                tableBody.empty();

                $.each(data, function (index, item) {

                    tableBody.append(`<tr>
                <td class="d-none"> ${item.caseId} </td>
                <td> ${item.employeeCode} </td>
                <td> ${item.firstName} </td>
                <td> ${item.lastName} </td>
                <td> ${new Date(item.dateCreated).toDateString()} </td>
                <td><button class="btn btn-primary btn-sm" onclick="getCaseDetails($(this))"> View Details </button></td>
                </tr>`);
                });
                initializeDatatable('datatable');
            })
            .fail(function (error) {
                console.log(error);
            });
    },

    getDisciplinaryCaseById: function (id) {

        $.post("/DisciplinaryCases/GetDisciplinaryCaseByCaseId/", {caseId: id})
            .done(function (response) {
                console.log(response);
                if (!response.success) {
                    swal({title: 'ERROR', type: 'error', text: response.message})
                    return;
                }
                if (response.payload === null || response.payload.length === 0) {
                    swal({title: 'No Data Found', type: 'info', text: 'Failed to retrieve disciplinary details'})
                    return;
                }
                let data = response.payload;
                $('.dateOfOffence').val(new Date(data.dateOffenceCommitted).toDateString());
                $('.caseType').val(data.caseType);
                $('.category').val(data.category);
                $('.outCome').val(data.caseOutcome);
                $('.caseDescription').val(data.caseDescription);
                $('#disciplinaryCaseDetailsModal').modal({backdrop: 'static', keyboard: false});
            })
            .fail(function (error) {
                console.log(error);
                swal({title: 'ERROR', type: 'error', text: 'There was an error processing your request'})
            });
    }
}

$(function () {
    methods.getAllDisciplinaryCases();
})

let getCaseDetails = function (currentObject) {

    let currentRow = currentObject.closest('tr');
    let caseId = Number($.trim(currentRow.find('td:eq(0)').text()));

    if (caseId === 0) {
        swal({title: 'ERROR', type: 'error', text: 'Case Id not provided'})
        return;
    }
    methods.getDisciplinaryCaseById(caseId);
}