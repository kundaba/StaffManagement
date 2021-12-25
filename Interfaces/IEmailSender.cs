using System.Threading.Tasks;

namespace CDFStaffManagement.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
