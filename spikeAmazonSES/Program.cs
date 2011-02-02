using System;
using System.Collections.Generic;
using Amazon.Runtime;
using Amazon.SimpleEmail;

namespace spikeAmazonSES
{
	public class Program {
		static void Main(string[] args) {

			ValidationResult validationResult = Validation.ParseAndValidateArguments(args);
			AmazonSimpleEmailService amazonSimpleEmailService = spikeAmazonSES.Execute.GetEmailService(validationResult.Credentials);

			if (validationResult.Option != ExecuteOptions.UnknownOption) {				
					Execute(validationResult, amazonSimpleEmailService);
				}
			else {
					Output.ShowUnknown();
			}			
		}

		public static void Execute(ValidationResult validationResult, AmazonSimpleEmailService amazonSimpleEmailService) {

			if (validationResult.Option == ExecuteOptions.Help) {
				Output.ShowHelp();
				return;
			}

			if (amazonSimpleEmailService == null) {
				Output.ShowCredentialsError();
				return;
			}

			Dictionary<ExecuteOptions, IExecute> services = GetServices();

			AmazonWebServiceResponse response = null;
			if (services.ContainsKey(validationResult.Option)) {				
				try 
				{
						response = services[validationResult.Option].ExecuteService(validationResult, amazonSimpleEmailService);
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

		private static Dictionary<ExecuteOptions, IExecute> GetServices() {
			return new Dictionary<ExecuteOptions, IExecute>
			{
				{ExecuteOptions.ListVerifiedEmailAddresses,new ExecuteListVerifiedEmailAddresses()},
				{ExecuteOptions.VerifyEmailAddress,new ExecuteVerifyEmailAddress()},
				{ExecuteOptions.DeleteVerifiedEmailAddress,new ExecuteDeleteVerifiedEmailAddress()},
				{ExecuteOptions.SendStatistics,new ExecuteSendStatistics()},
				{ExecuteOptions.SendQuota, new ExecuteSendQuota()},
				{ExecuteOptions.SendEmail, new ExecuteSendEmail()},
				{ExecuteOptions.SendRawEmail, new ExecuteSendRawEmail()}
			};
		}
	}
}
