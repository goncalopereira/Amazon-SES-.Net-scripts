using System.Collections.Generic;
using System.Linq;

namespace AmazonSESDotNetLib
{
	public class SendEmailValidation : Validation, IValidation
	{
		public ValidationResult Validation(string[] args) {
	
			if (HasOptionSubject(args) && HasOptionF(args)) {
				return new ValidationResult
				{
					SenderEmail = GetOption(args, "-f", 1),
					EmailAddresses = GetOption(args, "-f", 2).Split(',').ToList(),
					Subject = GetOption(args, "-s", 1),
					CC = HasOptionC(args) ? GetOption(args, "-c", 1).Split(',').ToList() : new List<string>(),
					BCC = HasOptionB(args) ? GetOption(args, "-b", 1).Split(',').ToList() : new List<string>(),
					Body = GetBody(args[args.Length - 1]),
					Option = ExecuteOptions.SendEmail
				};
			}

			if (HasOptionR(args)) {
				return new ValidationResult
				{
					Option = ExecuteOptions.SendRawEmail,
					Body = GetBody(args[args.Length - 1])
				};
			}

			return null;
		}
	}
}