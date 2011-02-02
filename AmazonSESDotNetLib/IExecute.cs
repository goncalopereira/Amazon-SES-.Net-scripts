using Amazon.Runtime;
using Amazon.SimpleEmail;

namespace AmazonSESDotNetLib
{
	public interface IExecute
	{
		AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService);
	}
}