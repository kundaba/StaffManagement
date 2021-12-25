using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Services.DisciplinaryCases.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface IDisciplinaryCaseService
    {
        Task<ResponseModel> AddDisciplinaryCase(DisciplinaryCaseDto request);
        Task<ResponseModel> GetDisciplinaryCases();
        
        Task<ResponseModel> GetDisciplinaryCaseByCaseId(int id);
    }
}