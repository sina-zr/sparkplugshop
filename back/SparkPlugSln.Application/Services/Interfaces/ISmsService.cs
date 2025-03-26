namespace SparkPlugSln.Application.Services.Interfaces;

public interface ISmsService
{
	Task<bool> SendPublicSMS(string phoneNumber, string message);
	Task<bool> SendLookupSMS(string phoneNumber, string templateName, string token1, string? token2 = "", string? token3 = "");
}