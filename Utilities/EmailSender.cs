using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CDFStaffManagement.Interfaces;

namespace CDFStaffManagement.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailSettings> _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }
        private async Task Execute(string email, string subject, string message)
        {
            try
            {
                var toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.Value.ToEmail
                                 : email;
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Value.UsernameEmail!, _emailSettings.Value.UsernameEmail?.Split('@')[0])
                };

                mail.To.Add(new MailAddress(toEmail!));
                mail.CC.Add(new MailAddress(_emailSettings.Value.CcEmail!));
                mail.Subject = "MyPayroll System - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using var smtp = new SmtpClient(_emailSettings.Value.PrimaryDomain, (int)_emailSettings.Value.PrimaryPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.Value.UsernameEmail, _emailSettings.Value.UsernamePassword),
                    EnableSsl = true
                };
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
