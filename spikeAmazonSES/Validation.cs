using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace spikeAmazonSES
{
	public class Validation
	{
		public static ValidationResult ParseAndValidateArguments(string[] args) {

			if (HasOptionH(args)) {
				return new ValidationResult {Option = ExecuteOptions.Help};
			}

			List<ValidationResult> commands = new List<ValidationResult>
			{
				VerifyEmailAddressValidation(args),
				GetStatsValidation(args),
				SendEmailValidation(args)
			};

			ValidationResult firstOrDefault = commands.Where(x => x != null).FirstOrDefault();
			if (firstOrDefault != null) {
				
				if (HasOptionE(args)) {
					firstOrDefault.EndPoint = GetOption(args, "-e", 1);
				}

				firstOrDefault.Credentials = GetCredentials(args);
				firstOrDefault.Verbose = HasOptionVerbose(args);

				return firstOrDefault;
			}

			return new ValidationResult {Option = ExecuteOptions.UnknownOption};
		}
		
		private static ValidationResult SendEmailValidation(string[] args) {

			if (args[0] != "ses-send-email.pl")
				return null;

			if (HasOptionSubject(args) && HasOptionF(args))				
			{	
				return new ValidationResult 
				{
					SenderEmail = GetOption(args, "-f", 1),
					EmailAddresses = GetOption(args,"-f",2).Split(',').ToList(),
					Subject = GetOption(args,"-s",1),				
					CC = HasOptionC(args) ? GetOption(args, "-c", 1).Split(',').ToList() : new List<string>(),
					BCC = HasOptionB(args) ? GetOption(args,"-b",1).Split(',').ToList() : new List<string>(),
					Body = GetBody(args[args.Length-1]),
					Option = ExecuteOptions.SendEmail
				};}

			if (HasOptionR(args)) {
				return new ValidationResult
				{
					Option = ExecuteOptions.SendRawEmail, Body = GetBody(args[args.Length-1])
				};
			}

			return null;
		}
		
		private static ValidationResult GetStatsValidation(string[] args) {
			
			if (args[0] != "ses-get-stats.pl")
				return null;

			if (HasOptionQ(args)) {
				return new ValidationResult{Option = ExecuteOptions.SendStatistics};
			}
			
			if (HasOptionStats(args)) {
				return new ValidationResult{Option = ExecuteOptions.SendQuota};
			}

			return null;
		}

		private static ValidationResult VerifyEmailAddressValidation(string[] args) {
			if (args[0] != "ses-verify-email-address.pl") {
				return null;
			}

			if (HasOptionL(args)) {
				return new ValidationResult{Option = ExecuteOptions.ListVerifiedEmailAddresses};
			}

			if (HasOptionV(args)) {
				return new ValidationResult {SenderEmail = GetOption(args, "-v", 1),Option = ExecuteOptions.VerifyEmailAddress};
			}

			if (HasOptionD(args)) {
				return new ValidationResult{SenderEmail = GetOption(args, "-d", 1),Option = ExecuteOptions.DeleteVerifiedEmailAddress};
			}

			return null;
		}

		private static bool HasOptionE(string[] args) {
			return args.Where((s, index) => s == "-e" && args.Length > index + 1).Any();
		}

		private static bool HasOptionC(string[] args) {
			return args.Where((s, index) => s == "-c" && args.Length > index + 1).Any();
		}

		private static bool HasOptionB(string[] args) {
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


		private static string GetBody(string path) {
			return File.Exists(path) ? File.ReadAllText(path) : path;
		}

		private static string GetOption(string[] args, string s, int i) {
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

		private static bool HasOptionR(string[] args) {
			return args.Where((s, index) => s == "-d" && args.Length > index + 1).Any();
		}

		private static bool HasOptionF(IList<string> args) {
			return args.Where((s, index) => s == "-f" && args.Count > index + 2).Any();
		}

		private static bool HasOptionStats(IEnumerable<string> args) {
			return args.Any(s => s == "-s");
		}

		private static bool HasOptionQ(IEnumerable<string> args) {
			return args.Any(s => s == "-q");
		}

		private static bool HasOptionD(string[] args) {
			return args.Where((s, index) => s == "-d" && args.Length > index + 1).Any();
		}

		private static bool HasOptionSubject(IList<string> args) {
			return args.Where((s, index) => s == "-s" && args.Count > index + 1).Any();
		}

		private static bool HasOptionV(string[] args) {
			return args.Where((s, index) => s == "-v" && args.Length > index + 1).Any();
		}

		private static bool HasOptionL(IEnumerable<string> args) {
			return args.Any(s => s == "-l");
		}
	}
}