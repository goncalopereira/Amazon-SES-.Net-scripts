using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmazonSESDotNetLib
{
	public class SendEmailValidationSF : Validation, IValidation
	{
		private const string CC = "-c";
		private const string BCC = "-b";
		private const string EMAIL_ADDRESSES = "-f";
		private const string SUBJECT = "-s";

		protected static string GetBody(string path) {
			return File.Exists(path) ? File.ReadAllText(path) : path;
		}

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, SUBJECT, 1) && HasOptionWithArguments(args, EMAIL_ADDRESSES, 2)
			       	? new ValidationResult
			       	{
			       		SenderEmail = GetOption(args, EMAIL_ADDRESSES, 1),
			       		EmailAddresses = GetOption(args, EMAIL_ADDRESSES, 2).Split(',').ToList(),
			       		Subject = GetOption(args, SUBJECT, 1),
			       		CC =
			       			HasOptionWithArguments(args, CC, 1)
			       				? GetOption(args, CC, 1).Split(',').ToList()
			       				: new List<string>(),
			       		BCC =
			       			HasOptionWithArguments(args, BCC, 1)
			       				? GetOption(args, BCC, 1).Split(',').ToList()
			       				: new List<string>(),
			       		Body = GetBody(args[args.Length - 1]),
			       		Option = ExecuteOptions.SendEmail
			       	}
			       	: null;
		}
	}

	public class SendEmailValidationR : SendEmailValidationSF {
		private const string RAW = "-r";

		public new ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, RAW, 1)
			       	? new ValidationResult
			       	{
			       		Option = ExecuteOptions.SendRawEmail,
			       		Body = GetBody(args[args.Length - 1])
			       	}
			       	: null;
		}
	}
}