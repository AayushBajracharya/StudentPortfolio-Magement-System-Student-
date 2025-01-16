using Application.Dto.CustomerMessage;
using Application.Interfaces.Repositories.CustomerMessageRepository;
using Application.Interfaces.Services.CustomerMessageService;

namespace Infrastructure.Persistence.Services.CustomerMessageService
{
    public class CustomerMessageService : ICustomerMessageService
    {
        private readonly ICustomerMessageRepository _repository;
        private readonly Application.Interfaces.Services.EmailSenderService.IEmailSender _emailSender;

        // Constructor to inject dependencies (repository and email sender)
        public CustomerMessageService(ICustomerMessageRepository repository, Application.Interfaces.Services.EmailSenderService.IEmailSender emailSender)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        // Method to save customer message and send email
        public async Task<CustomerMessageDto> SaveCustomerMessageAsync(CustomerMessageDto message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Message cannot be null.");

            // Map DTO to domain entity
            var customerMessage = new CustomerMessage
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                Email = message.Email,
                PhoneNumber = message.PhoneNumber,
                Message = message.Message,
                CreatedAt = DateTime.UtcNow
            };

            // Save the customer message to the database
            await _repository.AddAsync(customerMessage);
            await _repository.SaveChangesAsync();

            // Prepare the email body with actual data
            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Customer Detail</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f9f9f9;"">
    <table style=""width: 600px; margin: 20px auto; border-collapse: collapse; background-color: #ffffff; border: 1px solid #ddd; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
        <tr style=""background-color: #004085; color: #ffffff; text-align: center;"">
            <td style=""padding: 20px; font-size: 24px; font-weight: bold;"">
                New Customer Message
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px; font-size: 16px; color: #333333;"">
                <p>Dear Admin,</p>
                <p>You have received a message from your Online Health Care Application.</p>
            </td>
        </tr>
        <tr>
            <td style=""padding: 15px; text-align: left; font-size: 18px; font-weight: bold; color: #333; border-bottom: 2px solid #ddd;"">
                Customer Details
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px; font-size: 16px; color: #555555; line-height: 1.6;"">
                <p><strong>Customer Name:</strong> {message.FirstName} {message.LastName}</p>
                <p><strong>Email Address:</strong> <a href=""mailto:{message.Email}"" style=""color: #004085;"">{message.Email}</a></p>
                <p><strong>Phone Number:</strong> {message.PhoneNumber}</p>
                <p><strong>Message:</strong></p>
                <blockquote style=""margin: 0; padding: 10px; border-left: 4px solid #004085; background-color: #f5f5f5;"">
                    {message.Message}
                </blockquote>
            </td>
        </tr>
        <tr>
            <td style=""border-top: 1px solid #ddd;""></td>
        </tr>
        <tr>
            <td style=""background-color: #f9f9f9; color: #666; padding: 15px; text-align: center; font-size: 14px; border-top: 1px solid #ddd;"">
                Check out the <a href=""https://localhost:7067/api/HealthTest/GetAll"" style=""color: #0a5ca9; text-decoration: none;"">Online Health Care</a> if you'd like to learn more.
            </td>
        </tr>
    </table>
</body>
</html>";

            // Send the email with the email sender service
            await _emailSender.SendEmailAsync(
                 to: "healthcaretestkutumba@gmail.com", // Replace with the actual admin email address
                 subject: "New Customer Message",
                 body: emailBody
             );
            return message;
        }

      
    }
}
