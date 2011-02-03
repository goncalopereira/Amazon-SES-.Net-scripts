namespace AmazonSESDotNetLib
{
	public class GetStatsValidationS : Validation, IValidation
	{
		private const string STATS = "-s";

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, STATS, 0) ? new ValidationResult { Option = ExecuteOptions.SendQuota } : null;
		}
	}

	public class GetStatsValidationQ : Validation, IValidation {
		private const string QUOTA = "-q";

		public ValidationResult Validation(string[] args) {
			return HasOptionWithArguments(args, QUOTA, 0) ? new ValidationResult { Option = ExecuteOptions.SendStatistics } : null;
		}
	}
}