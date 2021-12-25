namespace CDFStaffManagement.Enums
{
    public static class ResponseConstants
    {
        public const string EmployeeCodeNotProvided = "EmployeeCode not Provided";
        public const string EmployeeNotFound= "Employee not found";
        public const string UserNotFound = "User not found";
        public const string UserNameNotProvided = "Username can not be null";
        public const string SearchCriteriaNotProvided = "Search Term not Provided";
        public const string RequiredDataNotProvided = "Please provide all the mandatory data in a valid format";
        public const string SomethingWentWrong = "Something went wrong while processing your request..Please try again";
        public const string EmployeeAddedSuccessfully = "Employee Added Successfully";
        public const string WrongUserNameOrPassword = "Wrong username or password provided";
        public const string PasswordIsNotAlphanumeric = "Password must contain Alphanumeric characters";
        public const string UserName = "UserName";
        public const string Decrypt = "Decrypt";
        public const string Encrypt = "Encrypt";
        public const string Success = "success";
        public const string Error = "error";
        public const string PasswordResetSuccessful = "Your Password has successfully been reset";
        public const string UserNameDoesNotExist = "The username/email_address provided does not exist or the account is locked/disabled";
        public const string UserAccountCreatedSuccessfully = "Account has successfully been created";
        public const string UserNameAlreadyTaken = "The supplied username is already taken. Please use a different one";
        public const string EmployeeCodeNotFound = "The EmployeeCode provided does not belong to any active employee";
        public const string RecordAddedSuccessfully = "Record Added SuccessfulLy";
        public const string PayslipLinesUploadedSuccessfully = " line(s) uploaded SuccessfulLy";
        public const string RecordNotFound = "Record Not Found";
        public const string RecordEditedSuccessfully = "Record Edited Successfully";
        public const string PayrollLineRemoved = "Payroll Line removed Successfully";
        public const string FieldsAmendedSuccessfully = "Field(s) Amended Successfully";
        public const string RecordAlreadyExists = "The provided record already exists";
        public const string InvalidPayrollDefinition = "Invalid payroll definition provided";
        public const string Deductions = "Deductions";
        public const string Earnings = "Earnings";
        public const string Percentage = "Percentage";
        public const string FixedAmount = "FixedAmount";
        public const string Vacant = "Vacant";
        public const string Filled = "Filled";
        public const string Update = "Update";
        public const string FailedToFetchPayslipDetails = "Failed to fetch payslip details for the selected employee";
        public const string ProvideValidStartDate = "Please provide valid start date";
        public const string PayrollLineNotFound = "Payroll line not found";
        public const string NewEmployee = "New Employee";
        public const string PositionNotFound = "Position not found or it's already linked to another employee";
        public const string EmployeeLinkedSuccessfully = "Employee linked to position successfully";
        public const string PositionCodesAddedSuccessfully = " position code(s) created successfully";
        public const string FailedToLinkEmployeeToPosition = "Failed to link employee to position";
        public const string EmployeeTerminatedSuccessfully = "Employee terminated successfully";
        public const string EmployeeReinstatedSuccessfully = "Employee reinstated successfully";
        public const string EmployeePromotedSuccessfully = "Employee promotion effected successfully";
        public const string FailedTransaction = "Transaction failed";
        public const string TransactionSuccessful = "Transaction Successful";
        public const string TimesheetAddedSuccessfully = "Timesheet Added Successfully";
        public const string TimesheetApprovedSuccessfully = "Timesheet Approved Successfully";
        public const string TimesheetAlreadyExist = "This employee already has a timesheet for the selected date";
        public const string CaseIdNotProvided = "Disciplinary case id not provided";
        public const string DocumentsUploadedSuccessfully = "Document(s) Uploaded Successfully";
        public const string LeaveTypeAlreadyExists = "This leave type already exists";
        public const string LeaveTypeAddedSuccessfully = "Leave Type Added Successfully";
        public const string EmployeeAlreadyHasLeaveDetail = "Employee already has the leave entitlement defined";

    }
}