using System;
using System.IO;
using System.Text;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace spikeAmazonSES
{
	public class ExecuteSendQuota : Execute
	{
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			return emailService.GetSendQuota(new GetSendQuotaRequest());
		}
	}

	public class ExecuteSendStatistics : Execute
	{
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			return emailService.GetSendStatistics(new GetSendStatisticsRequest());
		}
	}

	public class ExecuteDeleteVerifiedEmailAddress : Execute
	{
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			emailService.DeleteVerifiedEmailAddress(new DeleteVerifiedEmailAddressRequest().WithEmailAddress(param.SenderEmail));
			return null;
		}
	}

	public class ExecuteVerifyEmailAddress : Execute
	{
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			emailService.VerifyEmailAddress(new VerifyEmailAddressRequest().WithEmailAddress(param.SenderEmail));
			return null;
		}
	}

	public class ExecuteListVerifiedEmailAddresses : Execute {
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			return emailService.ListVerifiedEmailAddresses(new ListVerifiedEmailAddressesRequest());
		}
	}

	public class ExecuteSendRawEmail : Execute {
		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(param.Body.ToCharArray()));
			emailService.SendRawEmail(new SendRawEmailRequest().WithRawMessage(new RawMessage(memoryStream)));		
			return null;
		}
	}

	public class ExecuteSendEmail : Execute {

		public override AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			var destination = new Destination()
					.WithToAddresses(param.EmailAddresses)
					.WithCcAddresses(param.CC)
					.WithBccAddresses(param.BCC);

			emailService.SendEmail(new SendEmailRequest(param.SenderEmail,destination, GetMessage(param.Subject, param.Body)));

			return null;
		}

		protected static Message GetMessage(string subjectString, string bodyString) {
			Content subject = new Content((subjectString));
			Body body = new Body(new Content(bodyString));
			return new Message(subject, body);
		}
	}
	
	public abstract class Execute : IExecute
	{	
		public static AmazonSimpleEmailService GetEmailService(Credentials credentials) {
			return credentials != null ? new AmazonSimpleEmailServiceClient(credentials.Key, credentials.Secret) : null;
		}

		public virtual AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService) {
			throw new NotImplementedException();
		}
	}
}