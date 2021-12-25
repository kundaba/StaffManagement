using System.Collections.Generic;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Employee.Dto;

namespace CDFStaffManagement.Interfaces
{
    public interface IQualificationDocumentsService
    {
        Task<ResponseModel> UploadEmployeeQualification(QualificationSubmissionDto qualification);
        Task<EmployeeQualificationsView> GetDocument(string guid);
        Task<List<QualificationDocumentsDto>> GeEmployeeQualifications(string employeeCode);
        Task<ResponseModel> UpdateUploadedDocuments(List<long> documentIds, int employeeId);
    }
}