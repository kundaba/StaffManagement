using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;
using CDFStaffManagement.Services.PositionCodeDetails.Dtos;

namespace CDFStaffManagement.Services.PositionCodeDetails
{
    public class PositionCodeCodeDetailsService : IPositionCodeDetailsService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;

        public PositionCodeCodeDetailsService(MyPayrollContext dbContext, ICustomLogger customLogger)
        {
            _dbContext = dbContext;
            _customLogger = customLogger;
        }
       
        /**
         * Get all position codes
         */
        public async Task<ResponseModel> GetPositionCodes()
        {
            try
            {
                var positionCodes =
                    await _dbContext.PositionCodesView
                        .OrderByDescending(x => x.CreatedDate).
                        ToListAsync();
                var data = positionCodes ?? new List<PositionCodesView>();
                return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, data);
            }
            catch (Exception e)
            {
                _customLogger.ErrorLog(e.Message);
                return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, e.Message);
            }
        }
        /**
         * Method used to search for a particular position code
         */
        public async Task<List<string>> GetVacantPositionCode(string searchTerm)
        {
            var positionCodes = await _dbContext.PositionDetails
                .Where(x => x.Status == ResponseConstants.Vacant).ToListAsync();

            var positionList = (
                from item in positionCodes
                select new PositionCodeDto
                {
                    PositionCode = item.PositionCode + " - " + item.LongDescription
                }).ToList();

            var data = positionList.Where(x => x.PositionCode!.ToUpper().Contains(searchTerm.ToUpper()))
                .Select(y => y.PositionCode!.ToUpper()).Take(5).ToList();
            return data;
        }
        
        public async Task<List<string>> GetVacantAndFilledPositionCode(string searchTerm)
        {
            var positionCodes = await _dbContext.PositionDetails
                .Where(x => x.Status == ResponseConstants.Filled || x.Status == ResponseConstants.Vacant).ToListAsync();

            var positionList = (
                from item in positionCodes
                select new PositionCodeDto
                {
                    PositionCode = item.PositionCode + " - " + item.LongDescription
                }).ToList();

            var data = positionList.Where(x => x.PositionCode!.ToUpper().Contains(searchTerm.ToUpper()))
                .Select(y => y.PositionCode!.ToUpper()).Take(5).ToList();
            return data;
        }
        /**
         * This method is used to create a new position Code
         */

        public async Task<ResponseModel> CreatePositionCode(int numOfPositionCodes, string jobTitleCode)
        {
            var stringBuilder = new StringBuilder();

            var position = await _dbContext.PositionDetails.OrderByDescending(x => x.CreatedBy)
                .Where(x => x.JobTitleCode == jobTitleCode)
                .FirstOrDefaultAsync();
            
            var code = jobTitleCode.Trim().ToUpper();
            var numericValue = 0;
            if (position != null)
            {
                code = position.JobTitleCode!.Trim();
                numericValue = int.Parse(position.PositionCode!.Substring(4));
            }

            for (var i = 0; i < numOfPositionCodes; i++)
            {
                numericValue += 1;
                var positionCode = code + numericValue.ToString("D6");
                stringBuilder.Append(positionCode + " ");
            }

            return ResponseEntity.GetResponse(ResponseConstants.Success, 200, true, stringBuilder.ToString());
        }
        
        /**
         * Method for adding/saving newly created position codes
         */
        public async Task<ResponseModel> SaveCreatedPositionCode(PositionCodeDto positionCodeDto)
        {
            if (positionCodeDto == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var jobTitle = await _dbContext.JobTitle.Where(x => x.Jobcode == positionCodeDto.JobTitleCode)
                .FirstOrDefaultAsync();

            if (jobTitle == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.PositionNotFound, 500, false);
            }
            try
            {
                return await SaveCreatedPositionCodes(positionCodeDto, jobTitle);
            }
            catch (Exception e)
            {
                _customLogger.ErrorLog(e.Message);
                return ResponseEntity.GetResponse(e.Message, 500, false);
            }
        }
        
        /**
         * Method for linking employee code to the position code
         */
        public async Task<ResponseModel> LinkEmployeeToPositionCode(string employeeCode, string positionCode)
        {
            var position = await _dbContext.PositionDetails
                .Where(x => x.PositionCode == positionCode && x.Status =="VACANT")
                .FirstOrDefaultAsync();

            if (position == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.PositionNotFound, 500, false);    
            }

            position.EmployeeCode = employeeCode.Trim();
            position.Status = ResponseConstants.Filled;
            await _dbContext.SaveChangesAsync();
            
            return ResponseEntity.GetResponse(ResponseConstants.EmployeeLinkedSuccessfully, 200, true);
        }

        /**
         * Method for linking a position code to supervising position
         */
        public async Task<ResponseModel> LinkPositionToSupervisor(string positionCode, string reportsToPosition)
        {
            var position = await _dbContext.PositionDetails
                .Where(x => x.PositionCode == positionCode)
                .FirstOrDefaultAsync();

            if (position == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.PositionNotFound, 500, false);
            }

            position.ReportsToPositionCode = reportsToPosition;
            await _dbContext.SaveChangesAsync();

            return ResponseEntity.GetResponse(ResponseConstants.EmployeeLinkedSuccessfully, 200, true);
        }
        private async Task<ResponseModel> SaveCreatedPositionCodes(PositionCodeDto positionCodeDto, JobTitle jobTitle )
        {
            var positionDetailsList = new List<PositionDetails>();
            var positionCodesArray = positionCodeDto.PositionCode!.Split(" ");

            var counter = 0;

            for (var i = 0; i < positionCodesArray.Length; i++)
            {
                var code = positionCodesArray[i].Trim();

                var positionCode = await _dbContext.PositionDetails
                    .Where(x => x.PositionCode == code)
                    .FirstOrDefaultAsync();

                if (positionCode != null) continue;

                var positionDetails = new PositionDetails
                {
                    JobTitleCode = positionCodeDto.JobTitleCode?.Trim().ToUpper(),
                    PositionCode = code,
                    ShortDescription = jobTitle.ShortDescription?.Trim(),
                    LongDescription = jobTitle.LongDescription?.Trim(),
                    DepartmentId = positionCodeDto.DepartmentId,
                    ReportsToPositionCode = positionCodeDto.ReportsToPositionCode?.Trim(),
                    Status = ResponseConstants.Vacant,
                    StartDate = DateTime.Now,
                    CreatedBy = _customLogger.GetCurrentUser(),
                    CreatedDate = DateTime.Now
                };

                positionDetailsList.Add(positionDetails);
                counter++;
            }

            await _dbContext.PositionDetails.AddRangeAsync(positionDetailsList);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(counter + ResponseConstants.PositionCodesAddedSuccessfully, 200,
                true);
        }
    }
}