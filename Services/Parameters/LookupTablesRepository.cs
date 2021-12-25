using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Model.DBContext;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.Parameters
{
    public class LookupTablesRepository
    {
        private readonly MyPayrollContext _dbContent;
        private readonly List<LookupData> _lookupDataList;
        public LookupTablesRepository(MyPayrollContext dbContext)
        {
            _dbContent = dbContext;
            _lookupDataList = new List<LookupData>();
        }

        public async Task<IList<LookupData>> GetReferenceData(string dataCategory)
        {
            if (string.IsNullOrEmpty(dataCategory))
            {
                return new List<LookupData>();
            }

            return dataCategory switch
            {
                "JobTitles" => await JobTitleList(),
                "GradeList" => await GradeList(),
                "JobGeneral" => await JobGeneralList(),
                "CountryList" => await CountryList(),
                "DepartmentList" => await DepartmentList(),
                "BankList" => await BankList(),
                "BranchList" => await BranchList(),
                "IDTypeList" => await IdTypeList(),
                "ContractType" => await ContractList(),
                "MaritalStatus" => await MaritalStatusList(),
                "EmployeeStatus" => await EmployeeStatus(),
                "TerminationReasons" => await TerminationReasons(),
                _ => null
            } ?? throw new InvalidOperationException();
        }
        private async Task<IList<LookupData>> JobTitleList()
        {
            var jobTitleList = await _dbContent.JobTitle.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in jobTitleList.Select(item => new LookupData
            {
                Id = item.JobTitleId,
                Description = item.LongDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> GradeList()
        {
            var gradeList = await _dbContent.JobGrade.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in gradeList.Select(item => new LookupData
            {
                Id = item.JobGradeId,
                Description = item.JobGradeDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> JobGeneralList()
        {
            var jobGeneralList = await _dbContent.JobGeneral.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in jobGeneralList.Select(item => new LookupData
            {
                Id = item.JobGeneralId,
                Description = item.LongDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> CountryList()
        {
            var countryList = await _dbContent.CountryNames.Where(x => x.Status ==StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in countryList.Select(item => new LookupData
            {
                Id = item.Id,
                Description = item.CountryName
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> DepartmentList()
        {
            var deptList = await _dbContent.Department.Where(x=>x.Status!.Equals(StatusCodes.A.ToString())).ToListAsync();

            foreach (var dataObj in deptList.Select(item => new LookupData
            {
                Id = item.DepartmentId,
                Description = item.LongDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> BankList()
        {
            var bankList = await _dbContent.Bank.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in bankList.Select(item => new LookupData
            {
                Id = item.BankId,
                Description = item.BankName
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> BranchList()
        {
            var branchList = await _dbContent.BankBranch.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in branchList.Select(item => new LookupData
            {
                Id = item.BranchId,
                Description = item.BranchName
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> IdTypeList()
        {
            var idTypeList = await _dbContent.IdnumberType.Where(x => x.Status == StatusCodes.A.ToString()).ToListAsync();

            foreach (var dataObj in idTypeList.Select(item => new LookupData
            {
                Id = item.IdnumberTypeId,
                Description = item.LongDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> ContractList()
        {
            var contractTypeList = await _dbContent.NatureOfContract.Where(x => x.Status == StatusCodes.A.ToString())
                .ToListAsync();

            foreach (var dataObj in contractTypeList.Select(item => new LookupData
            {
                Id = item.Id,
                Description = item.ContractTypeDecsription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        private async Task<IList<LookupData>> MaritalStatusList()
        {
            var maritalStatusList = await _dbContent.MaritalStatus.Where(x=>x.Status ==StatusCodes.A.ToString())
                .ToListAsync();

            foreach (var dataObj in maritalStatusList.Select(item => new LookupData
            {
                Id = item.MaritalStatusId,
                Description = item.LongDescription
            }))
            {
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        
        private async Task<IList<LookupData>> TerminationReasons()
        {
            var terminationReasons = await _dbContent.TerminationReason.
                Where(x => x.Status == StatusCodes.A.ToString()).ToArrayAsync();

            foreach (var item in terminationReasons)
            {
                var dataObj = new LookupData
                {
                    Id = item.TerminationReasonId,
                    Description = item.Description
                };
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
        
        private async Task<IList<LookupData>> EmployeeStatus()
        {
            var employeeStatus = await _dbContent.EmployeeStatus.ToArrayAsync();

            foreach (var item in employeeStatus)
            {
                var dataObj = new LookupData
                {
                    Id = item.StatusId,
                    Description = item.StatusDescription
                };
                _lookupDataList.Add(dataObj);
            }
            return _lookupDataList;
        }
    }
}
