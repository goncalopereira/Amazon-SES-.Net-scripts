using AmazonSESDotNetLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spikeAmazonSES;

namespace tests {

	[TestClass]
	public class ValidationTests {

		[TestMethod]
		public void help_argument_returns_help_result() {
			ValidationResult result = Validation.ParseAndValidateArguments(new[] { "-h"});

			Assert.AreEqual(result.Option,ExecuteOptions.Help);
		}
	}
}
