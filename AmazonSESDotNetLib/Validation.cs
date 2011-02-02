using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmazonSESDotNetLib
{
	public interface IValidation
	{
		ValidationResult Validation(string[] args);
	}

	public class Validation
	{
		public static ValidationResult ParseAndValidateArguments(string[] args) {

			if (HasOptionH(args)) {
				return new ValidationResult {Option = ExecuteOptions.Help};
			}

			Dictionary<string, IValidation> commands = GetCommands();

			if (commands.ContainsKey(args[0])) {

				ValidationResult result = commands[args[0]].Validation(args);

				if (result != null) {
					if (HasOptionE(args)) {
						result.EndPoint = GetOption(args, "-e", 1);
					}
					result.Credentials = GetCredentials(args);
					result.Verbose = HasOptionVerbose(args);

					return result;
				}
				return new ValidationResult { Option = ExecuteOptions.UnknownOption };
			}
	
			return new ValidationResult {Option = ExecuteOptions.UnknownOption};
		}

		private static Dictionary<string, IValidation> GetCommands() {
			return new Dictionary<string, IValidation>
			{
				{"ses-verify-email-address.pl", new VerifyEmailAddressValidation()},
				{"ses-get-stats.pl", new GetStatsValidation()},
				{"ses-send-email.pl", new SendEmailValidation()}
			};
		}

		private static bool HasOptionE(string[] args) {
			return args.Where((s, index) => s == "-e" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionC(string[] args) {
			return args.Where((s, index) => s == "-c" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionB(string[] args) {
			return args.Where((s, index) => s == "-b" && args.Length > index + 1).Any();
		}

		private static bool HasOptionVerbose(IEnumerable<string> args) {
			return args.Where((s, index) => s == "--verbose").Any();
		}

		private static bool HasOptionH(IEnumerable<string> args) {
			return args.Where((s, index) => s == "-h").Any();
		}

		private static Credentials GetCredentials(string[] args) {
			return HasCredentials(args) ? GetCredentialsFromArguments(args) : GetCredentialsFromEnv();
		}

		private static Credentials GetCredentialsFromEnv() {
			string filePath = Environment.GetEnvironmentVariable("AWS_CREDENTIALS_FILE");
			return File.Exists(filePath) ? Credentials.GetCredentialsFromFile(File.ReadAllLines(filePath)) : null;			
		}

		private static Credentials GetCredentialsFromArguments(string[] args) {
			var filePath = GetOption(args, "-k", 1);
			return File.Exists(filePath) ? Credentials.GetCredentialsFromFile(File.ReadAllLines(filePath)) : null;
		}

		protected static string GetBody(string path) {
			return File.Exists(path) ? File.ReadAllText(path) : path;
		}

		protected static string GetOption(string[] args, string s, int i) {
			for (int index = 0; index < args.Length; index++) {
				if (args[index] == s) {
					return args[index + i];
				}
			}

			return null;
		}

		public static bool HasCredentials(string[] args) {
			return args.Where((s, index) => s == "-k" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionR(string[] args) {
			return args.Where((s, index) => s == "-d" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionF(IList<string> args) {
			return args.Where((s, index) => s == "-f" && args.Count > index + 2).Any();
		}

		protected static bool HasOptionStats(IEnumerable<string> args) {
			return args.Any(s => s == "-s");
		}

		protected static bool HasOptionQ(IEnumerable<string> args) {
			return args.Any(s => s == "-q");
		}

		protected static bool HasOptionD(string[] args) {
			return args.Where((s, index) => s == "-d" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionSubject(IList<string> args) {
			return args.Where((s, index) => s == "-s" && args.Count > index + 1).Any();
		}

		protected static bool HasOptionV(string[] args) {
			return args.Where((s, index) => s == "-v" && args.Length > index + 1).Any();
		}

		protected static bool HasOptionL(IEnumerable<string> args) {
			return args.Any(s => s == "-l");
		}
	}
}