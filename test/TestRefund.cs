using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestRefund
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string REFUNDID = "dummy_refund_id";
		const string PAYMENTID = "dummy_payment_id";
		string paymentUrl = "api/v1/payments";
		string refundUrl = "api/v1/refunds";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllRefunds()
		{
			string filepath = "dummy_refund_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, refundUrl));
			var result = client.Refund.All();
			Assert.AreNotSame(null, result);
		}

		[Test]
		public void TestGetAllRefundsWithFilters()
		{
			string filepath = "dummy_refund_collection_filters.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 5);
			options.Add("skip", 2);
			string url = string.Format("{0}?count={1}&skip={2}", refundUrl, options["count"], options["skip"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Refund.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfRefunds(result, expectedJsonString);
		}

		[Test]
		public void TestRetrieveRefund()
		{
			string filepath = "dummy_refund.json";
			string url = string.Format("{0}/{1}", refundUrl, REFUNDID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Refund refund = client.Refund.Retrieve(REFUNDID);
			Assert.AreNotSame(null, refund);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertRefund(refund, expectedJsonString);
		}

		[Test]
		public void TestCreateRefund()
		{
			string filepath = "dummy_refund_create.json";
			IDictionary<string, object> attributes = new Dictionary<string, object>();
			attributes.Add("amount", 100);
			string url = string.Format("{0}/{1}/refunds", paymentUrl, PAYMENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Refund refund = client.Refund.Create(PAYMENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertRefund(refund, expectedJsonString);
		}
	}
}
