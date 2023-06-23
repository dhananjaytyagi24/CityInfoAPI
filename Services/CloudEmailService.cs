using System;
namespace CityInfo.Services
{
	public class CloudEmailService : IMailService
	{
        private string EmailTo = "abc@gmail.com";
        private string EmailFrom = "xyz@gmail.com";


        public void SendEmail(string subject, string message)
        {
            Console.WriteLine($"Mail from {EmailFrom} to {EmailTo} sent with + {nameof(CloudEmailService)}");
            Console.WriteLine($"Email Subject: {subject} and Email Message {message}");
        }
    }
}

