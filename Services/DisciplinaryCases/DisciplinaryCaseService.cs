using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.DisciplinaryCases.Dto;
using CDFStaffManagement.Utilities;

namespace CDFStaffManagement.Services.DisciplinaryCases
{
    public class DisciplinaryCaseService: IDisciplinaryCaseService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;
        private readonly ICustomLogger _customLogger;

        public DisciplinaryCaseService(MyPayrollContext dbContext, IEmployeeObjectRepository employeeObjectRepository, ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
            _customLogger = customLogger;
        }

        public async Task<ResponseModel> GetDisciplinaryCases()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Model.EntityModels.DisciplinaryCases, DisciplinaryCaseDto>();
            });
            var iMapper = config.CreateMapper();

            var disciplinaryCases = await _dbContext.DisciplinaryCases
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            var mappedList = disciplinaryCases.Select(item => iMapper.Map<DisciplinaryCaseDto>(item)).ToList();
            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, mappedList);
        }

        public async Task<ResponseModel> GetDisciplinaryCaseByCaseId(int caseId)
        {
            var disciplinaryCase = await _dbContext.DisciplinaryCases.FindAsync(caseId);

            if (disciplinaryCase is null)
            {
                return  ResponseEntity.GetResponse(ResponseConstants.Error, 200, true, new DisciplinaryCaseDto()); 
            }
            var disciplinaryCaseObj = new DisciplinaryCaseDto
            {
                EmployeeCode = disciplinaryCase.EmployeeCode,
                FirstName = disciplinaryCase.FirstName,
                LastName = disciplinaryCase.LastName,
                DateOffenceCommitted = disciplinaryCase.DateOffenceCommitted,
                CaseType = disciplinaryCase.CaseType,
                Category = disciplinaryCase.Category,
                CaseOutcome = disciplinaryCase.CaseOutcome,
                CaseDescription = disciplinaryCase.CaseDescription,
                DateCreated = disciplinaryCase.DateCreated,
                CreatedBy = disciplinaryCase.CreatedBy
            };
            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, disciplinaryCaseObj);
        }

        public async Task<ResponseModel> AddDisciplinaryCase(DisciplinaryCaseDto disciplinaryCaseDto)
        {
            if (disciplinaryCaseDto is null)
            {
                throw new Exception(ResponseConstants.EmployeeCodeNotFound);
            }

            var employee =
                await _employeeObjectRepository.GetActiveEmployee(disciplinaryCaseDto.EmployeeCode!.Trim());

            if (employee is null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }
            return await SaveDisciplinaryCase(disciplinaryCaseDto, employee);
        }
        private async Task<ResponseModel> SaveDisciplinaryCase(DisciplinaryCaseDto disciplinaryCaseDto, EmployeeDetail? employee)
        {
            var disciplinaryCase = new  Model.EntityModels.DisciplinaryCases
            {
                EmployeeId = employee!.EmployeeId,
                EmployeeCode = disciplinaryCaseDto.EmployeeCode,
                FirstName = employee!.FirstName,
                LastName = employee.LastName,
                DateOffenceCommitted = disciplinaryCaseDto.DateOffenceCommitted,
                CaseType = disciplinaryCaseDto.CaseType,
                Category = disciplinaryCaseDto.Category,
                CaseOutcome = disciplinaryCaseDto.CaseOutcome,
                CaseDescription = disciplinaryCaseDto.CaseDescription,
                DateCreated = DateTime.Now,
                CreatedBy = _customLogger.GetCurrentUser()
            };
            await _dbContext.DisciplinaryCases.AddAsync(disciplinaryCase);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.TimesheetAddedSuccessfully, 200, true);
        }
    }
}