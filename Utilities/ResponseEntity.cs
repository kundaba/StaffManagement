using System;
using CDFStaffManagement.Enums;
using MyPayroll.Enums;

namespace CDFStaffManagement.Utilities
{
    public static class ResponseEntity
    {
        public static ResponseModel GetResponse(string message, int status, bool success)
        {
            var responseEntity = new ResponseModel
            {
                Message = message,
                Status = status,
                Success = success
            };
            return responseEntity;
        }
        public static ResponseModel GetResponse(string message, int status, bool success, object data)
        {
            var responseEntity = new ResponseModel
            {
                Message = message,
                Status = status,
                Success = success,
                Payload =  data,
                
            };
            return responseEntity;
        }
    }
}