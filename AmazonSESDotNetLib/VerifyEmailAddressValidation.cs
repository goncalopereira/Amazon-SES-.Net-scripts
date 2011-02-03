namespace AmazonSESDotNetLib
{
	public class VerifyEmailAddressValidationL : Validation, IValidation
	{
		private const string LIST = "-l";

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, LIST, 0) ? new ValidationResult { Option = ExecuteOptions.ListVerifiedEmailAddresses } : null;
		}
	}

	public class VerifyEmailAddressValidationV : Validation, IValidation {
		private const string VERIFY = "-v";

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, VERIFY, 1) ? new ValidationResult { SenderEmail = GetOption(args, VERIFY, 1), Option = ExecuteOptions.VerifyEmailAddress } : null;
		}
	}

	public class VerifyEmailAddressValidationD : Validation, IValidation {
		private const string DELETE = "-d";

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, DELETE, 1) ? new ValidationResult { SenderEmail = GetOption(args, DELETE, 1), Option = ExecuteOptions.DeleteVerifiedEmailAddress } : null;
		}
	}


}