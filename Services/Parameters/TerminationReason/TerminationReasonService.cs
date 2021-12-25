using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Services.Parameters.Interfaces;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;
namespace CDFStaffManagement.Services.Parameters.TerminationReason
{
    public class TerminationReasonService : ITerminationReasonService
    {
        private readonly MyPayrollContext _dbContext;

        public TerminationReasonService(MyPayrollContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TerminationReasons>> GetTerminationReasons()
        {
            var terminationReasonList = new List<TerminationReasons>();
            var data = await _dbContext.TerminationReason.OrderByDescending(x=>x.TerminationReasonId).ToListAsync();

            if (!data.Any()) return terminationReasonList;

            foreach (var item in data)
            {
                var terminationReason = new TerminationReasons
                {
                    Code = item.Code,
                    Description = item.Description,
                    Status = item.Status
                };
                terminationReasonList.Add(terminationReason);
            }

            return terminationReasonList;
        }

        public async Task<ResponseModel> AddTerminationReason(TerminationReasons request)
        {
            if (request.Code == null || request.Description == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var entryCheck = await _dbContext.TerminationReason
                .Where(x => x.Code == request.Code || x.Description!.Contains(request.Description))
                .FirstOrDefaultAsync();

            if (entryCheck != null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordAlreadyExists, 500, false);
            }

            var terminationReason = new Model.EntityModels.TerminationReason
            {
                Code = request.Code,
                Description = request.Description,
                Status = "A",
                DateCreated = DateTime.Now
            };
            await _dbContext.TerminationReason.AddAsync(terminationReason);
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }
        
        public async Task<ResponseModel> EditTerminationReason(TerminationReasons request)
        {
            if (request.Code == null || request.Description == null || request.Status == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            var terminationReason = await _dbContext.TerminationReason
                .Where(x => x.Code == request.Code)
                .FirstOrDefaultAsync();

            if (terminationReason == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RecordNotFound, 500, false);
            }

            terminationReason.Description = request.Description;
            terminationReason.Status = request.Status;
            await _dbContext.SaveChangesAsync();
            return ResponseEntity.GetResponse(ResponseConstants.RecordEditedSuccessfully, 200, true);
        }
    }
}