using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestPaymentLink
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
        const string PAYMENTLINKID = "dummy_payment_link_id";
		readonly string paymentLinkURL = "/api/v1/payment_links";

        [Fact]
        public void TestGetAllPaymentLinks()
		{
			string filepath = "dummy_payment_link_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentLinkURL));
			var result = client.PaymentLink.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

        [Fact]
        public void TestGetAllPaymentLinkWithFilters()
		{
			string filepath = "dummy_payment_link_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", paymentLinkURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.PaymentLink.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

        [Fact]
        public void TestGetPaymentLinkById()
		{
			string filepath = "dummy_payment_link.json";
			string url = string.Format("{0}/{1}", paymentLinkURL, PAYMENTLINKID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			PaymentLink paymentLink = client.PaymentLink.Retrieve(PAYMENTLINKID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(paymentLink, expectedJsonString);
		}

        [Fact]
        public void TestCreatePaymentLink()
		{
			string filepath = "dummy_payment_link.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentLinkURL));
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("customer_id", "cust_2WmsOoSRZMWWkcZg");
			options.Add("due_date", 1539171804);
			options.Add("currency", "INR");
            options.Add("amount", 100);
			PaymentLink paymentLink = client.PaymentLink.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(paymentLink, expectedJsonString);
		}

        [Fact]
		public void TestCancelPaymentLink()
		{
			string filepath = "dummy_payment_link.json";
			string url = string.Format("{0}/{1}", paymentLinkURL, PAYMENTLINKID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			PaymentLink paymentLink = client.PaymentLink.Retrieve(PAYMENTLINKID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(paymentLink, expectedJsonString);

			string filepath2 = "dummy_payment_link.json";
			string cancel_url = string.Format("{0}/cancel", url);
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, cancel_url));
			paymentLink = paymentLink.Cancel();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(paymentLink, expectedJsonString);
		}

        [Fact]
		public void TestEmptyCancelPaymentLink()
		{
			string filepath = "dummy_payment_link.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentLinkURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.PaymentLink.Cancel());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

        [Fact]
        public void TestGetAllPaymentsForPaymentLink()
		{
			string filepath = "dummy_payment_link.json";
			string url = string.Format("{0}/{1}", paymentLinkURL, PAYMENTLINKID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			PaymentLink paymentLink = client.PaymentLink.Retrieve(PAYMENTLINKID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(paymentLink, expectedJsonString);

			string filepath2 = "dummy_payment_collection.json";
			string payments_url = string.Format("{0}/payments", url);
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, payments_url));
			var result = paymentLink.Payments();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(result, expectedJsonString);
		}

        [Fact]
		public void TestEmptyPaymentsForPaymentLink()
		{
			string filepath = "dummy_payment_link.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, paymentLinkURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.PaymentLink.Payments());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}
    }
}