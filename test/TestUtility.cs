using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestUtility
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		Client client;

		public void Init(string accessID, string secretKey)
		{
			client = new Client(accessID, secretKey);
		}

		[Test]
		public void TestVerifyPaymentSignature()
		{
			Init(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("payment_signature", "e70360e32919311d31cbc9b558ea9024715b816ce64293ffc992459a94daac42");
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			Assert.IsTrue(client.Utility.VerifyPaymentSignature(options));
		}

		[Test]
		public void TestVerifyWrongPaymentSignature()
		{
			Init(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("payment_signature", "wrong_signature");
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			var ex = Assert.Throws<SignatureVerificationError>(() => client.Utility.VerifyPaymentSignature(options));
			Assert.That(ex.Message, Is.EqualTo("message: Invalid signature passed\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidSignatureError));
			Assert.That(ex.Field, Is.EqualTo(null));
		}

		[Test]
		public void TestInvalidArgumentError()
		{
			Init(ACCESSID, SECRETKEY);
			IDictionary<string, string> options = new Dictionary<string, string>();
			options.Add("order_id", "dummy_order_id");
			options.Add("payment_id", "dummy_payment_id");
			var ex = Assert.Throws<InvalidRequestError>(() => client.Utility.VerifyPaymentSignature(options));
			Assert.That(ex.Message, Is.EqualTo("message: The arguments provided are invalid.\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidArgumentError));
			Assert.That(ex.Field, Is.EqualTo(null));
		}
	}
}
