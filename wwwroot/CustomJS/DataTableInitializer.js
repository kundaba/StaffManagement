$(document).ready(function () {
  
    $('#datatables').DataTable({
        "pagingType": "full_numbers",
        responsive: true,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search records",
        }
    });
})

let initializeDatatable = function (tableId){
    $('#'+tableId).DataTable({
        "pagingType": "full_numbers",
        responsive: true,
        language: {
            search: "_INPUT_",
            searchPlaceholder: "Search records",
        }
    });
}