using System;
namespace CityInfo.Services
{
	public interface IMailService
	{
		void SendEmail(string subject, string message);
	}
}

