using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Parameters;

namespace CDFStaffManagement.Interfaces
{
    public interface IParameterRepository
    {
        Task<ResponseModel> AddDepartment(Departments department);
        Task<ResponseModel> DisableDepartment(int dptId);
        Task<List<Departments>> GetDepartmentList();
        Task<ResponseModel> EditDepartment(Departments dptModel);
        Task<ResponseModel> AddJobGrade(JobGrades jobGrade);
        Task<ResponseModel> EditJobGrade(int gradeId, string description, string? status);
        Task<List<JobGrades>> GetJobGradeList();
        Task<List<JobTitles>> GetJobTitleList();
        Task<ResponseModel> AddJobTitle(JobTitles jobTitle);
        Task<ResponseModel> EditJobTitle(JobTitles jobTitleModel);
        Task<List<Banks>> BankList();
        Task<ResponseModel> AddBank(Banks bankModel);
        Task<ResponseModel> EditBank(Banks model);
        Task<List<Branch>> BranchList();
        Task<ResponseModel> AddBranch(Branch model);
        Task<ResponseModel> EditBranch(Branch model);
        Task<List<TaxDefinition>> GetTaxDefinitionList();
        Task<ResponseModel> AddTaxDefinitionDetail(TaxDefinition defModel);
        Task<ResponseModel> EditTaxDefinitionDetail(TaxDefinition defModel);
        Task<IList<LookupData>> GetLookupData(string dataCategory);
        Task<List<BankBranch>> GetBranchByBankId(int bankId);

    }
}
