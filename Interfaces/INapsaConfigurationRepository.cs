using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.NapsaConfiguration;

namespace CDFStaffManagement.Interfaces
{
    public interface INapsaConfigurationRepository
    {
        Task<List<NapsaConfigurationDto>> GetNapsaConfigurations();

        Task<ResponseModel> AddNewConfiguration(NapsaConfigurationDto request);
    }
}