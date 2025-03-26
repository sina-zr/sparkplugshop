using SparkPlugSln.Domain.Models.kavenegar;

namespace SparkPlugSln.Application.Services.Implementations;

using SparkPlugSln.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

public class SmsService : ISmsService
{
	private readonly IConfiguration _configuration;

	#region Ctor
	
	private readonly KavenegarInfoVm _kavenegarInfo;

	public SmsService(IOptions<KavenegarInfoVm> kavenegarInfo)
	{
		_kavenegarInfo = kavenegarInfo.Value;
	}

	#endregion

	public async Task<bool> SendPublicSMS(string phoneNumber, string message)
	{
		if (_kavenegarInfo.IsSmsEnabled)
		{
			try
            {
            	var api = new Kavenegar.KavenegarApi(_kavenegarInfo.ApiKey);
    
            	var result = await api.Send(_kavenegarInfo.Sender, phoneNumber, message);
    
            	if (result.Status == 1)
            	{
            		return true;
            	}
    
            	return false;
            }
            catch (Kavenegar.Core.Exceptions.ApiException ex)
            {
            	//throw new Exception(ex.Message);
            	return false;
            }
            catch (Kavenegar.Core.Exceptions.HttpException ex)
            {
            	//throw new Exception(ex.Message);
            	return false;
            }
		}
		else
		{
			return true;
		}
		
	}

	public async Task<bool> SendLookupSMS(string phoneNumber, string templateName, string token1, string? token2 = "", string? token3 = "")
	{
		if (_kavenegarInfo.IsSmsEnabled)
		{
			try
            {
            	var api = new Kavenegar.KavenegarApi(_kavenegarInfo.ApiKey);
    
            	var result = await api.VerifyLookup(phoneNumber, token1, token2, token3, templateName);
    
            	if (result.Status is 1 or 4 or 5 or 10)
            	{
            		return true;
            	}
    
            	return false;
            }
            catch (Kavenegar.Core.Exceptions.ApiException ex)
            {
            	//throw new Exception(ex.Message);
            	return false;
            }
            catch (Kavenegar.Core.Exceptions.HttpException ex)
            {
            	//throw new Exception(ex.Message);
            	return false;
            }
		}
		else
		{
			return true;
		}
	}
}
