using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestTransfer
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string TRANSFERID = "dummy_transfer_id";
		readonly string transferURL = "/api/v1/transfers";

		[Fact]
		public void TestGetAllTransfers()
		{
			string filepath = "dummy_transfer_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, transferURL));
			var result = client.Transfer.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllTransfersWithFilters()
		{
			string filepath = "dummy_transfer_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", transferURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Transfer.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetTransferById()
		{
			string filepath = "dummy_transfer.json";
			string url = string.Format("{0}/{1}", transferURL, TRANSFERID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Transfer transfer = client.Transfer.Retrieve(TRANSFERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(transfer, expectedJsonString);
		}

	}
}
