using System.Collections.Generic;

namespace spikeAmazonSES
{
	public class ValidationResult
	{
		public List<string> CC { get; set; }
		public List<string> BCC { get; set; }
		public string EndPoint { get; set; }
		public bool Verbose { get; set; }
		public Credentials Credentials { get; set; }
		public ExecuteOptions Option { get; set; }
		public string SenderEmail { get; set; }
		public List<string> EmailAddresses { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }		
	}
}