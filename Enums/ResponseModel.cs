using System;

namespace CDFStaffManagement.Enums
{
    public class ResponseModel
    {
        public  int Status { get; set; }
        public  string? Message { get; set; }
        public  bool? Success { get; set; }
        public object? Payload { get; set; }
    }
}