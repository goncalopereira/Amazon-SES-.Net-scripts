namespace spikeAmazonSES
{
	public class Credentials
	{
		private const string AWSACCESSKEYID = "AWSAccessKeyId";
		private const string AWSSECRETKEY = "AWSSecretKey";

		public string Key { get; set; }
		public string Secret { get; set; }

		public static Credentials GetCredentialsFromFile(string[] lines) {
			if (lines.Length != 2)
				return null;

			string[] key = lines[0].Split('=');
			string[] value = lines[1].Split('=');

			if (key.Length != 2 && value.Length != 2 && key[0] != AWSACCESSKEYID && key[1] != AWSSECRETKEY) {
				return null;
			}

			return new Credentials {Key = key[1].Trim(), Secret = value[1].Trim()};
		}
	}
}