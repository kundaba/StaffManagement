namespace CDFStaffManagement.Services.PayslipDetails.Dtos
{
    public class PayeDto
    {
        public int Id { get; set; }
        public string? BandDescription { get; set; }
        public decimal? LowerLimit { get; set; }
        public decimal? UperLimit { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percentage { get; set; }
    }
}