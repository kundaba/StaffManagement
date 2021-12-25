using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;


namespace CDFStaffManagement.Services.Parameters
{
    public class ParametersRepository : IParameterRepository
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;

        public ParametersRepository(MyPayrollContext dbContext, ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _customLogger = customLogger;
        }

        public async Task<List<Departments>> GetDepartmentList()
        {
            var departments = await _dbContext.Department.ToListAsync();
            var departmentList = new List<Departments>();

            foreach (var department in departments)
            {
                var departmentObj = new Departments
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentCode = department.DepartmentCode,
                    LongDescription = department.LongDescription,
                    Status = await GetStatusDescription(department.Status)
                };
                departmentList.Add(departmentObj);
            }

            return departmentList;
        }

        public async Task<ResponseModel> DisableDepartment(int dptId)
        {
            if (string.IsNullOrEmpty(dptId.ToString()))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var department = await _dbContext.Department.FindAsync(dptId);
            if (department == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            department.Status = "I";
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse("Record Deleted Successfully", 200, true);
        }

        public async Task<ResponseModel> AddDepartment(Departments department)
        {
            if (department.DepartmentCode == null || department.LongDescription == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var deptCount = await _dbContext.Department
                .Where(x => x.DepartmentCode == department.DepartmentCode ||
                            x.LongDescription!.Contains(department.LongDescription))
                .CountAsync();
            if (deptCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var dpt = new Department
            {
                DepartmentCode = department.DepartmentCode.ToUpper().Trim(),
                LongDescription = department.LongDescription.Trim(),
                Status = "A",
                LastChanged = DateTime.Now,
                ChangedByUser = _customLogger.GetCurrentUser()
            };
            await _dbContext.Department.AddAsync(dpt);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditDepartment(Departments dptModel)
        {
            var department = await _dbContext.Department.FindAsync(dptModel.DepartmentId);
            if (department == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            department.DepartmentCode = dptModel.DepartmentCode!.ToUpper().Trim();
            department.LongDescription = dptModel.LongDescription!.Trim();
            department.Status = dptModel.Status!.Trim();
            department.LastChanged = DateTime.Now;
            department.ChangedByUser = _customLogger.GetCurrentUser();

            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<List<JobGrades>> GetJobGradeList()
        {
            var gradeList = new List<JobGrades>();
            var grades = await _dbContext.JobGrade.ToListAsync();
            foreach (var item in grades)
            {
                var gradeObject = new JobGrades()
                {
                    JobGradeId = item.JobGradeId,
                    JobGradeCode = item.JobGradeCode,
                    JobGradeDescription = item.JobGradeDescription,
                    Status = await GetStatusDescription(item.Status),
                };
                gradeList.Add(gradeObject);
            }

            return gradeList;
        }

        public async Task<ResponseModel> AddJobGrade(JobGrades jobGrade)
        {
            if (jobGrade.JobGradeCode == null || jobGrade.JobGradeDescription == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var gradeCount = await _dbContext.JobGrade
                .Where(x => x.JobGradeCode == jobGrade.JobGradeCode ||
                            x.JobGradeDescription.Contains(jobGrade.JobGradeDescription))
                .CountAsync();
            if (gradeCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var jbg = new JobGrade()
            {
                JobGradeCode = jobGrade.JobGradeCode.ToUpper().Trim(),
                JobGradeDescription = jobGrade.JobGradeDescription.Trim(),
                Status = "A",
            };
            await _dbContext.JobGrade.AddAsync(jbg);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditJobGrade(int gradeId, string description, string? status)
        {
            if (string.IsNullOrEmpty(gradeId.ToString()) || string.IsNullOrEmpty(description) ||
                string.IsNullOrEmpty(status))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var jobGrade = await _dbContext.JobGrade.Where(x => x.JobGradeId == gradeId).FirstOrDefaultAsync();
            if (jobGrade == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }
            jobGrade.Status = status;
            jobGrade.JobGradeDescription = description;
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<List<JobTitles>> GetJobTitleList()
        {
            var jobTitles = await (from j in _dbContext.JobTitle
                join g in _dbContext.JobGrade
                    on j.JobGradeId equals g.JobGradeId
                join s in _dbContext.StatusDescription
                    on j.Status equals s.StatusCode
                select new JobTitles
                {
                    JobTitleId = j.JobTitleId,
                    JobCode = j.Jobcode,
                    ShortDescription = j.ShortDescription,
                    LongDescription = j.LongDescription,
                    JobGrade = g.JobGradeCode,
                    Status = s.StausDescription
                }).ToListAsync();

            return jobTitles;
        }

        public async Task<ResponseModel> AddJobTitle(JobTitles jobTitle)
        {
            if (jobTitle.JobCode == null || jobTitle.ShortDescription == null || jobTitle.LongDescription == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var jbtCount = await _dbContext.JobTitle
                .Where(x => x.Jobcode == jobTitle.JobCode ||
                            x.ShortDescription!.Contains(jobTitle.ShortDescription))
                .CountAsync();

            if (jbtCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var newJobTitle = new JobTitle()
            {
                Jobcode = jobTitle.JobCode.ToUpper().Trim(),
                ShortDescription = jobTitle.ShortDescription.Trim(),
                LongDescription = jobTitle.LongDescription.Trim(),
                JobGradeId = jobTitle.JobGradeId,
                Status = "A",
                LastChanged = DateTime.Now,
                ChangedByUser = _customLogger.GetCurrentUser()
            };
            await _dbContext.JobTitle.AddAsync(newJobTitle);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditJobTitle(JobTitles jobTitles)
        {
            if (string.IsNullOrEmpty(jobTitles.Status) || jobTitles.Status == "0")
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var jobTitle = await _dbContext.JobTitle.FindAsync(jobTitles.JobTitleId);
            if (jobTitle == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            jobTitle.ShortDescription = jobTitles.ShortDescription!.Trim();
            jobTitle.LongDescription = jobTitles.LongDescription!.Trim();
            jobTitle.LastChanged = DateTime.Now;
            jobTitle.Status = jobTitles.Status;
            jobTitle.ChangedByUser = _customLogger.GetCurrentUser();
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<List<Banks>> BankList()
        {
            var bankList = new List<Banks>();
            var banks = await _dbContext.Bank.ToListAsync();
            foreach (var item in banks)
            {
                var gradeObject = new Banks
                {
                    BankId = item.BankId,
                    BankCode = item.Code,
                    BankName = item.BankName,
                    Status = await GetStatusDescription(item.Status),
                };
                bankList.Add(gradeObject);
            }

            return bankList;
        }

        public async Task<ResponseModel> AddBank(Banks bankModel)
        {
            if (string.IsNullOrEmpty(bankModel.BankName))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var recordCount = await _dbContext.Bank
                .Where(x => x.BankName == bankModel.BankName ||
                            x.Code == bankModel.BankCode)
                .CountAsync();

            if (recordCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            if (bankModel.BankCode != null)
            {
                var bnk = new Bank
                {
                    Code = bankModel.BankCode.ToUpper().Trim(),
                    BankName = bankModel.BankName.Trim(),
                    LastChanged = DateTime.Now,
                    Status = "A",
                };
                await _dbContext.Bank.AddAsync(bnk);
            }

            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditBank(Banks model)
        {
            if (string.IsNullOrEmpty(model.Status) || model.Status == "0" || string.IsNullOrEmpty(model.BankName))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var bank = await _dbContext.Bank.FindAsync(model.BankId);
            if (bank == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            bank.BankName = model.BankName.Trim();
            bank.LastChanged = DateTime.Now;
            bank.Status = model.Status;
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<List<Branch>> BranchList()
        {
            var branches = await (from j in _dbContext.BankBranch
                join b in _dbContext.Bank
                    on j.BankId equals b.BankId
                join s in _dbContext.StatusDescription
                    on j.Status equals s.StatusCode
                select new Branch
                {
                    BankName = b.BankName,
                    BranchCode = j.BranchCode,
                    BranchName = j.BranchName,
                    Status = s.StausDescription
                }).ToListAsync();

            return branches;
        }

        public async Task<List<BankBranch>> GetBranchByBankId(int bankId)
        {
            var branchList = await _dbContext.BankBranch
                .Where(x => x.BankId == bankId && x.Status == StatusCodes.A.ToString())
                .Select(x => new BankBranch
                {
                    BranchId = x.BranchId,
                    BranchName = x.BranchName
                }).ToListAsync();

            return branchList;
        }

        public async Task<ResponseModel> AddBranch(Branch model)
        {
            if (string.IsNullOrEmpty(model.BankId.ToString()) || string.IsNullOrEmpty(model.BranchCode) ||
                string.IsNullOrEmpty(model.BranchName))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var recordCount = await _dbContext.BankBranch
                .Where(x => x.BranchName == model.BranchName ||
                            x.BranchCode == model.BranchCode)
                .CountAsync();
            if (recordCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var branch = new BankBranch
            {
                BankId = model.BankId,
                BranchCode = model.BranchCode.Trim(),
                BranchName = model.BranchName,
                LastChanged = DateTime.Now,
                Status = "A",
            };
            await _dbContext.BankBranch.AddAsync(branch);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditBranch(Branch model)
        {
            if (string.IsNullOrEmpty(model.Status) || model.Status == "0" || string.IsNullOrEmpty(model.BranchName))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var branch = await _dbContext.BankBranch.Where(x => x.BranchCode == model.BranchCode)
                .FirstOrDefaultAsync();
            if (branch == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            branch.BranchName = model.BranchName.Trim();
            branch.LastChanged = DateTime.Now;
            branch.Status = model.Status;
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        public async Task<List<TaxDefinition>> GetTaxDefinitionList()
        {
            var taxDefList = await (from t in _dbContext.TaxTableDefinition
                orderby t.LowerLimit descending
                join s in _dbContext.StatusDescription
                    on t.Status equals s.StatusCode
                select new TaxDefinition
                {
                    Id = t.Id,
                    BandDescription = t.BandDescription,
                    LowerLimit = t.LowerLimit,
                    UperLimit = t.UperLimit,
                    Amount = t.Amount,
                    Percentage = t.Percentage,
                    Status = s.StausDescription,
                    DateCreated = t.DateCreated
                }).ToListAsync();
            return taxDefList;
        }

        public async Task<ResponseModel> AddTaxDefinitionDetail(TaxDefinition defModel)
        {
            if (string.IsNullOrEmpty(defModel.BandDescription))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var recordCount = await _dbContext.TaxTableDefinition
                .Where(x => x.BandDescription!.Contains(defModel.BandDescription))
                .CountAsync();

            if (recordCount != 0)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var taxObj = new TaxTableDefinition
            {
                BandDescription = defModel.BandDescription.Trim(),
                LowerLimit = defModel.LowerLimit,
                UperLimit = defModel.UperLimit,
                Amount = defModel.Amount,
                Percentage = defModel.Percentage,
                StartDate = DateTime.Now,
                DateCreated = DateTime.Now,
                CreatedBy = _customLogger.GetCurrentUser(),
                Status = "A"
            };
            await _dbContext.TaxTableDefinition.AddAsync(taxObj);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> EditTaxDefinitionDetail(TaxDefinition defModel)
        {
            if (string.IsNullOrEmpty(defModel.BandDescription) || string.IsNullOrEmpty(defModel.Status))
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var taxTable = await _dbContext.TaxTableDefinition.FindAsync(defModel.Id);
            if (taxTable == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            taxTable.LowerLimit = defModel.LowerLimit;
            taxTable.UperLimit = defModel.UperLimit;
            taxTable.Amount = defModel.Amount;
            taxTable.Percentage = defModel.Percentage;
            taxTable.Status = defModel.Status;
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }

        private async Task<string> GetStatusDescription(string? statusCode)
        {
            try
            {
                return (await _dbContext.StatusDescription
                    .Where(x => x.StatusCode == statusCode)
                    .Select(x => x.StausDescription)
                    .FirstOrDefaultAsync())!.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IList<LookupData>> GetLookupData(string dataCategory)
        {
            if (string.IsNullOrEmpty(dataCategory))
            {
                return new List<LookupData>();
            }
            var repository = new LookupTablesRepository(_dbContext);
            return await repository.GetReferenceData(dataCategory);
        }
    }
}