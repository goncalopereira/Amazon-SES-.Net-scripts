namespace AmazonSESDotNetLib
{
	public class VerifyEmailAddressValidation : Validation, IValidation
	{
		public ValidationResult Validation(string[] args) {
		
			if (HasOptionL(args)) {
				return new ValidationResult { Option = ExecuteOptions.ListVerifiedEmailAddresses };
			}

			if (HasOptionV(args)) {
				return new ValidationResult { SenderEmail = GetOption(args, "-v", 1), Option = ExecuteOptions.VerifyEmailAddress };
			}

			if (HasOptionD(args)) {
				return new ValidationResult { SenderEmail = GetOption(args, "-d", 1), Option = ExecuteOptions.DeleteVerifiedEmailAddress };
			}

			return null;
		}
	}
}