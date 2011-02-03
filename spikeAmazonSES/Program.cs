using System;
using System.Collections.Generic;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using AmazonSESDotNetLib;

namespace spikeAmazonSES
{
	public class Program {
		static void Main(string[] args) {

			ValidationResult validationResult = Validation.ParseAndValidateArguments(args);
		
			AmazonSimpleEmailService amazonSimpleEmailService = Execute.GetEmailService(validationResult.Credentials);

			if (validationResult.Option != ExecuteOptions.UnknownOption) {				
					ExecuteService(validationResult, amazonSimpleEmailService);
				}
			else {
					Output.ShowUnknown();
			}			
		}

		public static void ExecuteService(ValidationResult validationResult, AmazonSimpleEmailService amazonSimpleEmailService) {

			if (validationResult.Option == ExecuteOptions.Help) {
				Output.ShowHelp();
				return;
			}

			if (amazonSimpleEmailService == null) {
				Output.ShowCredentialsError();
				return;
			}

			IExecute service = GetServiceFromOption(validationResult.Option);

			AmazonWebServiceResponse response = null;
			if (service != null) {				
				try 
				{
						response = service.ExecuteService(validationResult, amazonSimpleEmailService);
				} 
				catch (AmazonSimpleEmailServiceException e) {
					Console.WriteLine("{0}:{1}", e.ErrorType, e.Message);					
				}						

			} else {
				Output.ShowUnknown();
			}

			if (response != null) {
				Output.Write(response,validationResult.Verbose);
			}	
		}

		private static IExecute GetServiceFromOption(ExecuteOptions option) {
			var list = new Dictionary<ExecuteOptions, IExecute>
			{
				{ExecuteOptions.ListVerifiedEmailAddresses,new ExecuteListVerifiedEmailAddresses()},
				{ExecuteOptions.VerifyEmailAddress,new ExecuteVerifyEmailAddress()},
				{ExecuteOptions.DeleteVerifiedEmailAddress,new ExecuteDeleteVerifiedEmailAddress()},
				{ExecuteOptions.SendStatistics,new ExecuteSendStatistics()},
				{ExecuteOptions.SendQuota, new ExecuteSendQuota()},
				{ExecuteOptions.SendEmail, new ExecuteSendEmail()},
				{ExecuteOptions.SendRawEmail, new ExecuteSendRawEmail()}
			};

			return list.ContainsKey(option) ? list[option] : null;
		}
	}
}
