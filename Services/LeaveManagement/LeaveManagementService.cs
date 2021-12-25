using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.LeaveManagement.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.LeaveManagement
{
    public class LeaveManagementService : ILeaveManagementService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _logger;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;

        public LeaveManagementService(MyPayrollContext dbContext, ICustomLogger logger,
            IEmployeeObjectRepository employeeObjectRepository)
        {
            _dbContext = dbContext;
            _logger = logger;
            _employeeObjectRepository = employeeObjectRepository;
        }

        public async Task<List<LeaveTypesDto>> GetAllLeaveTypes()
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<LeaveTypes, LeaveTypesDto>(); });
            var iMapper = config.CreateMapper();

            var leaveTypes = await _dbContext.LeaveTypes
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();

            var mappedList = leaveTypes.Select(item => iMapper.Map<LeaveTypesDto>(item)).ToList();
            return mappedList;
        }

        public async Task<ResponseModel> GetLeaveDetailByEmployeeCode(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeCodeNotProvided, 500, false);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee is null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var leaveDetails = await _dbContext.LeaveEntitlementView
                .Where(x => x.EmployeeCode == employeeCode)
                .OrderBy(x=>x.LeaveTypeDescription)
                .ToListAsync();

            List<LeaveEntitlementViewDto> leaveEntitlementList = new List<LeaveEntitlementViewDto>();

            if (leaveDetails.Any())
            {
                leaveEntitlementList.AddRange(leaveDetails.Select(item => new LeaveEntitlementViewDto
                {
                    EmployeeCode = item.EmployeeCode,
                    LeaveAccrualStartDate = item.LeaveAccrualStartDate,
                    LeaveTypeDescription = item.LeaveTypeDescription,
                    Entitlement = item.Entitlement,
                    LeaveBalance = item.LeaveBalance,
                    MonetaryValue = item.MonetaryValue
                }));
            }

            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, leaveEntitlementList);
        }

        public async Task<ResponseModel> AddLeaveType(LeaveTypesDto leaveTypesDto)
        {
            if (leaveTypesDto is null)
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            var leaveType =
                await _dbContext.LeaveTypes.Where(x => x.Code!.Equals(leaveTypesDto.Code)
                                                       || x.LeaveTypeDescription!.Contains(leaveTypesDto.LeaveTypeDescription!))
                    .FirstOrDefaultAsync();

            if (leaveType is not null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.EmployeeNotFound, 500, false);
            }

            var levType = new LeaveTypes
            {
                Code = leaveTypesDto.Code,
                LeaveTypeDescription = leaveTypesDto.LeaveTypeDescription,
                Entitlement = leaveTypesDto.Entitlement,
                ApplicableGender = leaveTypesDto.ApplicableGender,
                BalanceBroughtForwardOption = leaveTypesDto.BalanceBroughtForwardOption,
                Cycle = leaveTypesDto.Cycle,
                DateCreated = DateTime.Now,
                CreatedBy = _logger.GetCurrentUser()
            };
            await _dbContext.LeaveTypes.AddAsync(levType);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.LeaveTypeAddedSuccessfully, 200, true);
        }

        public async Task<ResponseModel> AddAndUpdateLeaveDetail(LeaveDetailDto leaveDetailDto)
        {
            if (leaveDetailDto is null)
            {
                throw new Exception(ResponseConstants.EmployeeCodeNotFound);
            }

            var employeeLeaveDetail =
                await _dbContext.LeaveDetail
                    .Where(x => x.EmployeeId == leaveDetailDto.EmployeeId &&
                                x.LeaveTypeId == leaveDetailDto.LeaveTypeId)
                    .FirstOrDefaultAsync();

            decimal? monetaryValue = null;

            switch (LeaveDetailDto.ActionType)
            {
                case "Add":

                    if (employeeLeaveDetail is not null)
                    {
                        throw new Exception(ResponseConstants.EmployeeAlreadyHasLeaveDetail);
                    }

                    var leaveDetail = new LeaveDetail
                    {
                        EmployeeId = leaveDetailDto.EmployeeId,
                        LeaveAccrualStartDate = leaveDetailDto.LeaveAccrualStartDate,
                        LeaveBalance = leaveDetailDto.LeaveBalance,
                        MonetaryEquivalent = monetaryValue,
                        LeaveTypeId = leaveDetailDto.LeaveTypeId
                    };
                    await _dbContext.LeaveDetail.AddAsync(leaveDetail);
                    await _dbContext.SaveChangesAsync();
                    break;

                case "Update":

                    if (employeeLeaveDetail is null)
                    {
                        throw new Exception(ResponseConstants.EmployeeNotFound);
                    }

                    employeeLeaveDetail.LeaveBalance = leaveDetailDto.LeaveBalance;
                    employeeLeaveDetail.MonetaryEquivalent = monetaryValue;
                    await _dbContext.SaveChangesAsync();
                    break;
                default:
                    throw new Exception("Invalid action provided");
            }

            return ResponseEntity.GetResponse(ResponseConstants.LeaveTypeAddedSuccessfully, 200, true);
        }
    }
}