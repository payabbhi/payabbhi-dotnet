using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

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
			return VerifySignature(payload, expectedSignature);
		}

		bool VerifySignature(string payload, string expectedSignature)
		{
			string actualSignature = ComputeHash(Client.SecretKey, payload);
			if (String.Compare(actualSignature, expectedSignature) != 0)
			{
				throw new Error.SignatureVerificationError(Constants.Messages.InvalidSignatureError, null, null);
			}
			return true;
		}

		string ComputeHash(string secretKey, string payload)
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
