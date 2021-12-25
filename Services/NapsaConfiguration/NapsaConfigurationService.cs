using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDFStaffManagement.Services.NapsaConfiguration
{
    public class NapsaConfigurationService : INapsaConfigurationRepository
    {
        private readonly MyPayrollContext _dbContext;
        private readonly ICustomLogger _customLogger;

        public NapsaConfigurationService(MyPayrollContext dbContent, ICustomLogger customLogger)
        {
            _dbContext = dbContent;
            _customLogger = customLogger;
        }

        /**
         * get all the configurations
         */
        public async Task<List<NapsaConfigurationDto>> GetNapsaConfigurations()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Model.EntityModels.NapsaConfiguration, NapsaConfigurationDto>();
            });
            var iMapper = config.CreateMapper();
            var configurations = await _dbContext.NapsaConfiguration.ToListAsync();

            return configurations.Select(item => iMapper.Map<NapsaConfigurationDto>(item)).ToList();
        }

        /**
         * Add new line
         */
        public async Task<ResponseModel> AddNewConfiguration(NapsaConfigurationDto request)
        {
            if (request == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, false);
            }

            //expire the current active record
            await ExpireCurrentActiveConfig(request.StartDate);

            //create new line
            return await CreateNewConfigLine(request);
        }

        private async Task<ResponseModel> CreateNewConfigLine(NapsaConfigurationDto request)
        {
            _dbContext.Add(new Model.EntityModels.NapsaConfiguration
            {
                Percentage = request.Percentage,
                MaximumCeiling = request.MaximumCeiling,
                StartDate = request.StartDate,
                EndDate = null,
                CreatedDate = DateTime.Now,
                ModifiedBy = request.ModifiedBy
            });
            await _dbContext.SaveChangesAsync();

            return ResponseEntity.GetResponse(ResponseConstants.RecordAddedSuccessfully, 200, true);
        }

        private async Task ExpireCurrentActiveConfig(DateTime startDate)
        {
            
            var currentActiveLinesAsync =
                await _dbContext.NapsaConfiguration.Where(x => x.EndDate == null).ToListAsync();

            if (currentActiveLinesAsync.Any())
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync();
                var user = _customLogger.GetCurrentUser();
                foreach (var item in currentActiveLinesAsync)
                {
                    item.EndDate = GetEndDate(startDate);
                    item.ModifiedBy = user;
                    item.ModifiedDate = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
        }

        private static DateTime GetEndDate(DateTime startDate)
        {
            var today = DateTime.Now;
            var endDate = today;

            if (startDate < today)
            {
                endDate = startDate;
            }
            if (startDate > today)
            {
                endDate = startDate.AddDays(-1);
            }

            return endDate;
        }
    }
}