using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Payabbhi
{
	public class Utility
	{
		/// <summary>
		/// Verifies the payment signature.
		/// </summary>
		/// <returns><c>true</c>, if payment signature was verified, <c>false</c> otherwise.</returns>
		/// <param name="paymentResponse">Payment response.</param>
		public bool VerifyPaymentSignature(IDictionary<string, string> paymentResponse)
		{
			string expectedSignature = string.Empty;
			string orderID = string.Empty;
			string paymentID = string.Empty;
			string payload = string.Empty;
			try
			{
				expectedSignature = paymentResponse["payment_signature"];
				orderID = paymentResponse["order_id"];
				paymentID = paymentResponse["payment_id"];
				payload = paymentID + "&" + orderID;
			}
			catch (Exception)
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidArgumentError, null, null, HttpStatusCode.Unused);
			}
			return VerifySignature(payload, expectedSignature,Client.SecretKey);
		}

		/// <summary>
		/// Verifies the webhook signature.
		/// </summary>
		/// <returns><c>true</c>, if webhook signature was verified, <c>false</c> otherwise.</returns>
		/// <param name="payload">Payload.</param>
		/// <param name="actualSignature">Actual Signature.</param>
		/// <param name="secret">Secret.</param>
		/// <param name="replayInterval">Replay Interval.</param>
		public bool VerifyWebhookSignature(string payload, string actualSignature, string secret, int replayInterval = 300)
		{
			string expectedSignature = string.Empty;

			var signatureMap = actualSignature.Split(',')
							.Select (part  => part.Trim().Split('='))
							.Where (part => part.Length == 2)
							.ToDictionary (sp => sp[0], sp => sp[1]);

			Int32 currentTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			int signatureTimestamp=0;
			Int32.TryParse(signatureMap["t"], out signatureTimestamp);

			if ( !signatureMap.ContainsKey("t") || !signatureMap.ContainsKey("v1") || currentTimestamp - signatureTimestamp > replayInterval)
			{
				throw new Error.SignatureVerificationError(Constants.Messages.InvalidSignatureError, null, null);
			}
			string canonicalString = string.Format("{0}&{1}",payload,signatureMap["t"]);
			return VerifySignature(canonicalString, signatureMap["v1"],secret);
		}

		bool VerifySignature(string payload, string expectedSignature, string secret)
		{
			string actualSignature = ComputeHash(secret, payload);
			if (String.Compare(actualSignature, expectedSignature) != 0)
			{
				throw new Error.SignatureVerificationError(Constants.Messages.InvalidSignatureError, null, null);
			}
			return true;
		}

		public string ComputeHash(string secretKey, string payload)
		{
			var key = Encoding.UTF8.GetBytes(secretKey);
			string hashString;
			using (var hmac = new HMACSHA256(key))
			{
				var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
				hashString = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
			}
			return hashString;
		}
	}
}
