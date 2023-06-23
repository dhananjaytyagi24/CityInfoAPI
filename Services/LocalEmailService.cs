using System;
namespace CityInfo.Services
{
    public class LocalEmailService : IMailService
    {
        private readonly string EmailTo;
        private readonly string EmailFrom = "xyz@gmail.com";

        public LocalEmailService(IConfiguration configuration)
        {
            EmailTo = configuration.GetValue<string>("emailSettings:emailTo");
            EmailFrom = configuration.GetValue<string>("emailSettings:emailFrom");
        }

        public void SendEmail(string subject, string message)
        {
            Console.WriteLine($"Mail from {EmailFrom} to {EmailTo} sent with {nameof(LocalEmailService)}");
            Console.WriteLine($"Email Subject: {subject} and Email Message {message}");
        }
    }
}

