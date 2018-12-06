using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestPayment
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string PAYMENTID = "dummy_payment_id";
		readonly string paymentURL = "/api/v1/payments";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllPayments()
		{
			string filepath = "dummy_payment_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentURL));
			var result = client.Payment.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfPayments(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllPaymentsWithFilters()
		{
			string filepath = "dummy_payment_collection_filters.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 4);
			options.Add("skip", 2);
			string url = string.Format("{0}?count={1}&skip={2}", paymentURL, options["count"], options["skip"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Payment.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfPayments(result, expectedJsonString);
		}

		[Test]
		public void TestGetPaymentById()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);
		}

		[Test]
		public void TestGetPaymentWithEMIById()
		{
			string filepath = "dummy_payment_with_emi.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);
		}

		[Test]
		public void TestGetPaymentWithUPIById()
		{
			string filepath = "dummy_payment_with_upi.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);
		}

		[Test]
		public void TestCapturePayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);

			string capture_url = string.Format("{0}/capture", paymentURL);
			string filepath2 = "dummy_payment_capture.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, capture_url));
			payment = payment.Capture();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertPayment(payment, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentCapturesPayment()
		{
			string capture_url = string.Format("{0}/capture", paymentURL);
			string filepath = "dummy_payment_capture.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, capture_url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Capture());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestRefundPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);

			filepath = "dummy_refund_create.json";
			IDictionary<string, object> attributes = new Dictionary<string, object>();
			attributes.Add("amount", 100);
			url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Refund refund = payment.Refund();
			expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertRefund(refund, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentRefundsPayment()
		{
			string filepath = "dummy_refund_create.json";
			IDictionary<string, object> attributes = new Dictionary<string, object>();
			attributes.Add("amount", 100);
			string url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Refund());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestPaymentRetrieveRefunds()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);

			string retrieve_refunds_url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			string filepath2 = "dummy_payment_refunds.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, url));
			var refunds = payment.GetRefunds();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertListOfRefunds(refunds, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentRetrievesRefunds()
		{
			string filepath = "dummy_payment_refunds.json";
			string url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.GetRefunds());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestCreateTransfersAgainstPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, "pay_W2FmbqANt09epUOz");
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve("pay_W2FmbqANt09epUOz");
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);

			string create_url = string.Format("{0}/transfers", url);
			string filepath2 = "dummy_transfer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, create_url));
			Dictionary<string, object> transferItem1 = new Dictionary<string, object>() {
	      {"amount", 20 },
	      {"currency", "INR" },
	      {"description", "Tranfer 1" },
	      {"recipient_id", "recp_Y2ojRlJVqRMhB0Ay" }
      };
      Dictionary<string, object> transferItem2 = new Dictionary<string, object>()
      {
          {"amount", 30 },
          {"currency", "INR" },
          {"description", "Tranfer 2" },
          {"recipient_id", "recp_Y2ojRlJVqRMhB0Ay" }
      };
			Dictionary<string, object> transferItem3 = new Dictionary<string, object>()
      {
          {"amount", 50 },
          {"currency", "INR" },
          {"description", "Tranfer 3" },
          {"recipient_id", "recp_Y2ojRlJVqRMhB0Ay" }
      };
      Dictionary<string, object>[] transferArr = { transferItem1, transferItem2, transferItem3 };
			var transfers = payment.Transfer(new Dictionary<string, object>() {
      	{"transfers", transferArr }
      });
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertListOfTransfers(transfers, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentCreatesTransfers()
		{
			string url = string.Format("{0}/transfers", paymentURL);
			string filepath = "dummy_transfer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Transfer());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestListTransfersForPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, "pay_W2FmbqANt09epUOz");
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve("pay_W2FmbqANt09epUOz");
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertPayment(payment, expectedJsonString);

			string list_url = string.Format("{0}/transfers", url);
			string filepath2 = "dummy_transfer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, list_url));
			var transfers = payment.Transfers();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertListOfTransfers(transfers, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentListsTransfers()
		{
			string url = string.Format("{0}/transfers", paymentURL);
			string filepath = "dummy_transfer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Transfers());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}
	}
}
