namespace AmazonSESDotNetLib
{
	public class Credentials
	{
		private const string AWS_ACCESS_KEY_ID = "AWSAccessKeyId";
		private const string AWS_SECRET_KEY = "AWSSecretKey";

		public string Key { get; set; }
		public string Secret { get; set; }

		public static Credentials GetCredentialsFromFile(string[] lines) {
			if (lines.Length != 2)
				return null;

			string[] key = lines[0].Split('=');
			string[] value = lines[1].Split('=');

			if (key.Length != 2 && value.Length != 2 && key[0] != AWS_ACCESS_KEY_ID && key[1] != AWS_SECRET_KEY) {
				return null;
			}

			return new Credentials {Key = key[1].Trim(), Secret = value[1].Trim()};
		}
	}
}