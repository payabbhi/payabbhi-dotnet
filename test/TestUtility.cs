using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;
using System;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestUtility
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";

		[Fact]
		public void TestVerifyPaymentSignature()
		{
			Client client = new Client(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("payment_signature", "e70360e32919311d31cbc9b558ea9024715b816ce64293ffc992459a94daac42");
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			Assert.True(client.Utility.VerifyPaymentSignature(options));
		}

		[Fact]
		public void TestVerifyWrongPaymentSignature()
		{
			Client client = new Client(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("payment_signature", "wrong_signature");
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			var ex = Assert.Throws<SignatureVerificationError>(() => client.Utility.VerifyPaymentSignature(options));
			Assert.Equal(ex.Message, "message: Invalid signature\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidSignatureError);
			Assert.Equal(ex.Field, null);
		}

		[Fact]
		public void TestVerifyWebhookSignature()
		{
			Client client = new Client(ACCESSID, SECRETKEY);
			string filepath = "dummy_event.json";
			string payload = Helper.GetJsonString(filepath).Replace(System.Environment.NewLine, string.Empty);
			string secret = "skw_live_jHNxKsDqJusco5hA";
			Int32 t = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			string canonicalString = string.Format("{0}&{1}",payload,t);
			string v1 = client.Utility.ComputeHash(secret,canonicalString);

			string actualSignature = string.Format("t={0}, v1={1}",t,v1);

			Assert.True(client.Utility.VerifyWebhookSignature(payload,actualSignature,secret));
		}

		[Fact]
		public void TestVerifyWrongWebhookSignature()
		{
			Client client = new Client(ACCESSID, SECRETKEY);
			string filepath = "dummy_event.json";
			string payload = Helper.GetJsonString(filepath).Replace(System.Environment.NewLine, string.Empty);
			string secret = "skw_live_jHNxKsDqJusco5hA";
			string actualSignature = "t=1536577756, v1=random";
			var ex = Assert.Throws<SignatureVerificationError>(() => client.Utility.VerifyWebhookSignature(payload,actualSignature,secret));
			Assert.Equal(ex.Message, "message: Invalid signature\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidSignatureError);
			Assert.Equal(ex.Field, null);
		}

		[Fact]
		public void TestInvalidArgumentError()
		{
			Client client = new Client(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			var ex = Assert.Throws<InvalidRequestError>(() => client.Utility.VerifyPaymentSignature(options));
			Assert.Equal(ex.Message, "message: The arguments provided are invalid.\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidArgumentError);
			Assert.Equal(ex.Field, null);
		}
	}
}
