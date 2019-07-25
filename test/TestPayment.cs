using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestPayment
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string PAYMENTID = "dummy_payment_id";
		readonly string paymentURL = "/api/v1/payments";

		[Fact]
		public void TestGetAllPayments()
		{
			string filepath = "dummy_payment_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentURL));
			var result = client.Payment.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllPaymentsWithFilters()
		{
			string filepath = "dummy_payment_collection_filters.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 4);
			options.Add("skip", 2);
			string url = string.Format("{0}?count={1}&skip={2}", paymentURL, options["count"], options["skip"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Payment.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetPaymentById()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);
		}

		[Fact]
		public void TestGetPaymentWithEMIById()
		{
			string filepath = "dummy_payment_with_emi.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);
		}

		[Fact]
		public void TestGetPaymentWithUPIById()
		{
			string filepath = "dummy_payment_with_upi.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);
		}

		[Fact]
		public void TestCapturePayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);

			string capture_url = string.Format("{0}/capture", paymentURL);
			string filepath2 = "dummy_payment_capture.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, capture_url));
			payment = payment.Capture();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(payment, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentCapturesPayment()
		{
			string capture_url = string.Format("{0}/capture", paymentURL);
			string filepath = "dummy_payment_capture.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, capture_url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Capture());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestRefundPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);

			filepath = "dummy_refund_create.json";
			IDictionary<string, object> attributes = new Dictionary<string, object>();
			attributes.Add("amount", 100);
			url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Refund refund = payment.Refund();
			expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(refund, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentRefundsPayment()
		{
			string filepath = "dummy_refund_create.json";
			IDictionary<string, object> attributes = new Dictionary<string, object>();
			attributes.Add("amount", 100);
			string url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Refund());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestPaymentRetrieveRefunds()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);

			string retrieve_refunds_url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			string filepath2 = "dummy_payment_refunds.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, url));
			var refunds = payment.GetRefunds();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(refunds, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentRetrievesRefunds()
		{
			string filepath = "dummy_payment_refunds.json";
			string url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.GetRefunds());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestCreateTransfersAgainstPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);

			string create_url = string.Format("{0}/transfers", url);
			string filepath2 = "dummy_transfer_collection.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, create_url));
			Dictionary<string, object> transferItem1 = new Dictionary<string, object>() {
			  {"amount", 20 },
			  {"currency", "INR" },
			  {"description", "Tranfer 1" },
			  {"beneficiary_id", "recp_Y2ojRlJVqRMhB0Ay" }
		  };
			Dictionary<string, object> transferItem2 = new Dictionary<string, object>()
		  {
			  {"amount", 30 },
			  {"currency", "INR" },
			  {"description", "Tranfer 2" },
			  {"beneficiary_id", "recp_Y2ojRlJVqRMhB0Ay" }
		  };
			Dictionary<string, object> transferItem3 = new Dictionary<string, object>()
		  {
			  {"amount", 50 },
			  {"currency", "INR" },
			  {"description", "Tranfer 3" },
			  {"beneficiary_id", "recp_Y2ojRlJVqRMhB0Ay" }
		  };
			Dictionary<string, object>[] transferArr = { transferItem1, transferItem2, transferItem3 };
			var transfers = payment.Transfer(new Dictionary<string, object>() {
				{"transfers", transferArr }
		  });
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(transfers, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentCreatesTransfers()
		{
			string url = string.Format("{0}/transfers", paymentURL);
			string filepath = "dummy_transfer_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Transfer());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestListTransfersForPayment()
		{
			string filepath = "dummy_payment.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Retrieve(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);

			string list_url = string.Format("{0}/transfers", url);
			string filepath2 = "dummy_transfer_collection.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, list_url));
			var transfers = payment.Transfers();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(transfers, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentListsTransfers()
		{
			string url = string.Format("{0}/transfers", paymentURL);
			string filepath = "dummy_transfer_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Transfers());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestGetVirtualAccount()
		{
			string filepath = "dummy_payment_with_virtual_account.json";
			string url = string.Format("{0}/{1}", paymentURL, PAYMENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Payment payment = client.Payment.Details(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(payment, expectedJsonString);
		}
	}
}
