using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CDFStaffManagement.Enums;
using CDFStaffManagement.Interfaces;
using CDFStaffManagement.Model.DBContext;
using CDFStaffManagement.Model.EntityModels;
using CDFStaffManagement.Services.Employee.Dto;
using CDFStaffManagement.Utilities;
using Microsoft.EntityFrameworkCore;


namespace CDFStaffManagement.Services.Uploads
{
    public class QualificationDocumentsService : IQualificationDocumentsService
    {
        private readonly MyPayrollContext _dbContext;
        private readonly IEmployeeObjectRepository _employeeObjectRepository;

        public QualificationDocumentsService(MyPayrollContext dbContext,
            IEmployeeObjectRepository employeeObjectRepository)
        {
            _dbContext = dbContext;
            _employeeObjectRepository = employeeObjectRepository;
        }

        /**
         * This method is used to add/update employee qualifications during take-on
         */
        public async Task<ResponseModel> UploadEmployeeQualification(QualificationSubmissionDto qualification)
        {
            if (qualification.Document == null)
            {
                return ResponseEntity.GetResponse(ResponseConstants.RequiredDataNotProvided, 500, true);
            }

            List<long> fileIdList = new();
            var counter = 0;
            
            for (var i = 0; i < qualification.Document!.Length; i++)
            {
                var file = qualification.Document[i];
                var qualificationType = qualification.QualificationType![i];
                var fieldOfStudy = qualification.FieldOfStudy![i];
                var documentName = file.FileName;
                var startDate = qualification.StartDate![i];
                var endDate = qualification.EndDate![i];
                var documentType = qualification.DocumentType;

                if (file == null)
                {
                    continue;
                }

                Stream stream = file.OpenReadStream();
                BinaryReader binaryReader = new(stream);
                byte[] fileContent = binaryReader.ReadBytes((int)stream.Length);

                var fileId = await SaveQualification(qualificationType, fieldOfStudy, fileContent, documentName,
                    documentType!, startDate, endDate, qualification.ActionType, qualification.EmployeeCode!);
                fileIdList.Add(fileId);
                
                counter++;
            }
            return counter > 0
                ? ResponseEntity.GetResponse(ResponseConstants.DocumentsUploadedSuccessfully, 200, true, fileIdList)
                : ResponseEntity.GetResponse(ResponseConstants.Error, 500, false);
        }

        /**
         * Get a qualification by GUID
         */
        public async Task<EmployeeQualificationsView> GetDocument(string guid)
        {
            if (guid is null)
            {
                throw new Exception(ResponseConstants.RequiredDataNotProvided);
            }

            var qualification =
                await _dbContext.EmployeeQualificationsView
                    .Where(x => x.GuId == guid)
                    .FirstOrDefaultAsync();
            return qualification ?? new EmployeeQualificationsView();
        }

        /**
         * Get qualifications for a given employee
         */
        public async Task<List<QualificationDocumentsDto>> GeEmployeeQualifications(string employeeCode)
        {
            if (employeeCode is null)
            {
                throw new Exception(ResponseConstants.EmployeeCodeNotProvided);
            }

            var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);

            if (employee is null)
            {
                throw new Exception(ResponseConstants.EmployeeNotFound);
            }

            var qualificationList =
                await _dbContext.EmployeeQualifications
                    .Where(x => x.EmployeeId == employee.EmployeeId)
                    .ToListAsync();

            List<QualificationDocumentsDto> documentList = new();

            if (!qualificationList.Any())
            {
                return documentList;
            }

            foreach (var item in qualificationList)
            {
                var docStatus = "Expired Document";
                QualificationDocumentsDto qualificationDocumentsDto = new()
                {
                    GuId = item.GuId,
                    DocumentName = item.DocumentName,
                    FieldOfStudy = item.FieldOfStudy,
                    QualificationType = item.QualificationType,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate
                };
                if (item.EndDate == null || item.EndDate > DateTime.Now)
                {
                    docStatus = "Valid Document";
                }

                qualificationDocumentsDto.ExpiryStatus = docStatus;
                documentList.Add(qualificationDocumentsDto);
            }
            return documentList;
        }

        public async Task<ResponseModel> UpdateUploadedDocuments(List<long> documentIds, int employeeId)
        {
            if (!documentIds.Any())
            {
                return ResponseEntity.GetResponse(ResponseConstants.Error, 500, false);
            }

            var counter = 0;
            foreach (var id in documentIds)
            {
                var document = await _dbContext.EmployeeQualifications.FindAsync(id);

                if (document is null)
                {
                    continue;
                }

                document.EmployeeId = employeeId;
                await _dbContext.SaveChangesAsync();
                counter++;
            }

            return counter > 0
                ? ResponseEntity.GetResponse(ResponseConstants.Success, 200, true)
                : ResponseEntity.GetResponse(ResponseConstants.Error, 500, false);
        }

        private async Task<long> SaveQualification(string? qualificationType, string? fieldOfStudy, byte[] fileContent,
            string? documentName, string documentType, DateTime startDate, DateTime endDate, string action, string employeeCode)
        {
            int? employeeId = null;

            if (action.Equals(ResponseConstants.Update))
            {
                var employee = await _employeeObjectRepository.GetActiveEmployee(employeeCode);
                employeeId = employee.EmployeeId;
            }

            EmployeeQualifications employeeQualification = new()
            {
                GuId = Guid.NewGuid().ToString(),
                EmployeeId = employeeId,
                DocumentType = documentType,
                QualificationType = qualificationType,
                FieldOfStudy = fieldOfStudy,
                DocumentContent = fileContent,
                DocumentName = documentName,
                StartDate = startDate,
                EndDate = endDate
            };
            await _dbContext.EmployeeQualifications.AddAsync(employeeQualification);
            await _dbContext.SaveChangesAsync();
            return employeeQualification.FileId;
        }
    }
}