using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmazonSESDotNetLib
{
	public class Validation
	{
		private const string VERBOSE = "--verbose";
		private const string CREDENTIALS = "-k";
		private const string HELP = "-h";
		private const string VERIFY_EMAIL_ADDRESS_COMMAND = "ses-verify-email-address.pl";
		private const string GET_STATS_COMMAND = "ses-get-stats.pl";
		private const string SEND_EMAIL_COMMAND = "ses-send-email.pl";

		public static ValidationResult ParseAndValidateArguments(string[] args) {

			if (HasOptionWithArguments(args, HELP, 0)) {
				return new ValidationResult {Option = ExecuteOptions.Help};
			}

			IValidation command = GetValidationsForCommand(args[0]).Where(x => x.Validation(args) != null).FirstOrDefault();

			if (command != null) {
				ValidationResult result = command.Validation(args);

				result.Credentials = GetCredentials(args);
				result.Verbose = HasOptionWithArguments(args, VERBOSE, 0);

				return result;
			}
		
			return new ValidationResult {Option = ExecuteOptions.UnknownOption};
		}

		private static IEnumerable<IValidation> GetValidationsForCommand(string command) {
			var commands = new Dictionary<string, List<IValidation>>
			{
				{VERIFY_EMAIL_ADDRESS_COMMAND,
					new List<IValidation>{
						new VerifyEmailAddressValidationL(),
						new VerifyEmailAddressValidationV(),
						new VerifyEmailAddressValidationD()}},
				{GET_STATS_COMMAND,
					new List<IValidation> {new GetStatsValidationS(), new GetStatsValidationQ()}},
				{SEND_EMAIL_COMMAND,
					new List<IValidation>() {new SendEmailValidationSF(), new SendEmailValidationR()}}
			};
			return commands.ContainsKey(command) ? commands[command] : new List<IValidation>();
		}

		protected static bool HasOptionWithArguments(string[] args, string optionName, int numberOfArguments) {
			return args.Where((s, index) => s == optionName && args.Length > index + numberOfArguments).Any();
		}

		private static Credentials GetCredentials(string[] args) {
			return HasOptionWithArguments(args, CREDENTIALS, 1) ? GetCredentialsFromArguments(args) : GetCredentialsFromEnv();
		}

		private static Credentials GetCredentialsFromEnv() {
			string filePath = Environment.GetEnvironmentVariable("AWS_CREDENTIALS_FILE");
			return File.Exists(filePath) ? Credentials.GetCredentialsFromFile(File.ReadAllLines(filePath)) : null;			
		}

		private static Credentials GetCredentialsFromArguments(string[] args) {
			var filePath = GetOption(args, CREDENTIALS, 1);
			return File.Exists(filePath) ? Credentials.GetCredentialsFromFile(File.ReadAllLines(filePath)) : null;
		}

		protected static string GetOption(string[] args, string s, int i) {
			for (int index = 0; index < args.Length; index++) {
				if (args[index] == s) {
					return args[index + i];
				}
			}
			return null;
		}
	}
}