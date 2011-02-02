namespace AmazonSESDotNetLib
{
	public class GetStatsValidation : Validation, IValidation
	{
		public ValidationResult Validation(string[] args) {
	
			if (HasOptionQ(args)) {
				return new ValidationResult { Option = ExecuteOptions.SendStatistics };
			}

			if (HasOptionStats(args)) {
				return new ValidationResult { Option = ExecuteOptions.SendQuota };
			}

			return null;
		}
	}
}