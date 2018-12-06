using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestTransfer
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string TRANSFERID = "dummy_transfer_id";
		readonly string transferURL = "/api/v1/transfers";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllTransfers()
		{
			string filepath = "dummy_transfer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, transferURL));
			var result = client.Transfer.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfTransfers(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllTransfersWithFilters()
		{
			string filepath = "dummy_transfer_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", transferURL, options["count"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Transfer.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfTransfers(result, expectedJsonString);
		}

		[Test]
		public void TestGetTransferById()
		{
			string filepath = "dummy_transfer.json";
			string url = string.Format("{0}/{1}", transferURL, TRANSFERID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Transfer transfer = client.Transfer.Retrieve(TRANSFERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertTransfer(transfer, expectedJsonString);
		}

	}
}
