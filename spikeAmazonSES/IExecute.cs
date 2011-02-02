using Amazon.Runtime;
using Amazon.SimpleEmail;

namespace spikeAmazonSES
{
	public interface IExecute
	{
		AmazonWebServiceResponse ExecuteService(ValidationResult param, AmazonSimpleEmailService emailService);
	}
}