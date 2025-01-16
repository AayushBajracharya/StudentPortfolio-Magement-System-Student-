using Application.Interfaces.Services.EmailSenderService;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Persistence.Services.EmailSenderService
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["Smtp:Host"],
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["Smtp:Username"],
                    _configuration["Smtp:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:FromEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set this to true to enable HTML rendering
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
