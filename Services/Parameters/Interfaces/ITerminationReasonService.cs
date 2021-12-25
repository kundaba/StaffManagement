using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;

namespace CDFStaffManagement.Services.Parameters.Interfaces
{
    public interface ITerminationReasonService
    {
        Task<List<TerminationReasons>> GetTerminationReasons();

        Task<ResponseModel> AddTerminationReason(TerminationReasons request);

        Task<ResponseModel> EditTerminationReason(TerminationReasons request);
    }
}