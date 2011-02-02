using System;
using Amazon.Runtime;
using Amazon.SimpleEmail.Model;

namespace spikeAmazonSES
{
	public class Output
	{	
		public static void ShowHelp() {
			Console.WriteLine("Help");
			Console.WriteLine("Mandatory on every command: -k flag followed by filename containing credentials OR from AWS_CREDENTIALS_FILE env variable");
			Console.WriteLine("Optional on every command: --verbose flag for XML output if the command has one");
			Console.WriteLine("---");
			Console.WriteLine("ses-verify-email-address.pl -l - List of verified e-mail addresses");
			Console.WriteLine("ses-verify-email-address.pl -v EMAIL_ADDRESS - send verification request to e-mail address - NO OUTPUT");
			Console.WriteLine("ses-verify-email-address.pl -d EMAIL_ADDRESS - delete e-mail address - NO OUTPUT");
			Console.WriteLine("---");
			Console.WriteLine("ses-get-stats.pl -q - Send Quota information");
			Console.WriteLine("ses-get-stats.pl -s - Send Statistics information");
			Console.WriteLine("---");
			Console.WriteLine("ses-send-email.pl -s SUBJECT -f SENDER_EMAIL \"RECEIVER_EMAIL1,RECEIVER_EMAIL2,...\" [-b \"BCC_EMAIL1, BCC_EMAIL2...\"] [-c \"CC_EMAIL1, CC_EMAIL2...\"] BODY_FILE - send e-mail - NO OUTPUT");
			Console.WriteLine("ses-send-email.pl -r RAW_MESSAGE_FILE - send e-mail using raw message - NO OUTPUT");
		}

		public static void ShowUnknown() {
			Console.WriteLine("Unknown combination of flags, -h for usage");
		}

		public static void ShowCredentialsError() {
			Console.WriteLine("Error reading credentials, -h for usage");
		}

		public static void Write(AmazonWebServiceResponse response, bool verbose) {
			
			if (response is GetSendStatisticsResponse) {
				Write((GetSendStatisticsResponse)response, verbose);
			}

			if (response is ListVerifiedEmailAddressesResponse) {
				Write((ListVerifiedEmailAddressesResponse) response,verbose);
			}

			if (response is GetSendQuotaResponse) {
				Write((GetSendQuotaResponse)response, verbose);
			}
		}

		public static void Write(GetSendQuotaResponse responseStats, bool verbose) {
			if (verbose) {
				var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(GetSendQuotaResponse));
				xmlSerializer.Serialize(Console.Out, responseStats);
			} else {
				Console.WriteLine("Max24HourSend: " + responseStats.GetSendQuotaResult.Max24HourSend);
				Console.WriteLine("MaxSendRate: " + responseStats.GetSendQuotaResult.MaxSendRate);
				Console.WriteLine("SentLast24Hours: " + responseStats.GetSendQuotaResult.SentLast24Hours);
			}
		}

		public static void Write(GetSendStatisticsResponse responseStats, bool verbose) {
			if (verbose) {
				var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(GetSendStatisticsResponse));
				xmlSerializer.Serialize(Console.Out, responseStats);
			} else {
				foreach (SendDataPoint dataPoint in responseStats.GetSendStatisticsResult.SendDataPoints) {
					Console.WriteLine(
						"Timestamp: {0} Delivery Attempts: {1} Bounces:{2} Rejects: {3} Complaints: {4}",
						dataPoint.Timestamp, dataPoint.DeliveryAttempts, dataPoint.Bounces,
						dataPoint.Rejects, dataPoint.Complaints);
				}
			}
		}

		public static void Write(ListVerifiedEmailAddressesResponse listResponse, bool verbose) {

			if (verbose) {
				var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ListVerifiedEmailAddressesResponse));
				xmlSerializer.Serialize(Console.Out, listResponse);
			} else {
				foreach (string emailAddress in listResponse.ListVerifiedEmailAddressesResult.VerifiedEmailAddresses) {
					Console.WriteLine(emailAddress);
				}
			}
		}
	}
}