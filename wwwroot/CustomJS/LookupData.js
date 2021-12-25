
$(document).ready(function () {

        getLookupData('JobTitles', 'jobTitle');
        getLookupData('GradeList', 'grade');
        getLookupData('JobGeneral', 'jobGeneral');
        getLookupData('CountryList', 'nationality');
        getLookupData('DepartmentList', 'department');
        getLookupData('BankList', 'bank');
        getLookupData('BranchList', 'branch');
        getLookupData('IDTypeList', 'IDType');
        getLookupData('ContractType', 'contractType');
        getLookupData('MaritalStatus', 'maritalStatus');
        
/*    $('#bank').on('change', function () {
        if ($(this).val() !== "") {
          $.post("/Parameters/GetBranchByBankId", { bankId: $(this).val() }, "json")
             .done(function (data) {
                    $('#branch').empty();
                    console.log(data);
                  if (data.length > 0) {
                   $.each(data, function (i, item) {
                         $("#branch").append(`<option value = ${item.branchId} > ${item.branchName}</option >`).selectpicker('refresh');
                       });
              }
              })
             .fail(function (error) {
                console.log(error);
                });
       }
    });*/
})

let getLookupData = function(dataCategory,txtField) {
    $.post("/Parameters/GetLookupData", { dataCategory: dataCategory}, "json")
        .done(function (data) {
            if (data.length > 0) {
                $.each(data, function (i, item) {
                    $("#" + txtField).append("<option value = " + item.id + " > " + item.description + "</option >").selectpicker('refresh');
                });
            }
        })
        .fail(function (error) {
            console.log(error);
        });
}