using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestVirtualAccount
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string VIRTUALACCOUNTID = "dummy_virtual_account_id";
		readonly string virtualAccountURL = "/api/v1/virtual_accounts";

		[Fact]
		public void TestGetAllVirtualAccount()
		{
			string filepath = "dummy_virtual_account_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, virtualAccountURL));
			var result = client.VirtualAccount.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllVirtualAccountWithFilters()
		{
			string filepath = "dummy_virtual_account_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", virtualAccountURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.VirtualAccount.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetVirtualAccountById()
		{
			string filepath = "dummy_virtual_account.json";
			string url = string.Format("{0}/{1}", virtualAccountURL, VIRTUALACCOUNTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			VirtualAccount virtualAccount = client.VirtualAccount.Retrieve(VIRTUALACCOUNTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(virtualAccount, expectedJsonString);
		}

		[Fact]
		public void TestCreateVirtualAccount()
		{
			string filepath = "dummy_virtual_account.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, virtualAccountURL));
			string[] collectionMethod = { "bank_account" };
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("invoice_id", "invt_srxOZZk6dIgWTVls");
			options.Add("collection_methods", collectionMethod);
			options.Add("customer_id", "cust_2WmsQoSRZMWWkcZg");
			options.Add("email", "test@example.com");
			options.Add("notification_method", "both");
			options.Add("customer_notification_by", "platform");
			VirtualAccount virtualAccount = client.VirtualAccount.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(virtualAccount, expectedJsonString);
		}

		[Fact]
		public void TestDeleteVirtualAccount()
		{
			string filepath = "dummy_virtual_account.json";
			string url = string.Format("{0}/{1}", virtualAccountURL, VIRTUALACCOUNTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			VirtualAccount virtualAccount = client.VirtualAccount.Retrieve(VIRTUALACCOUNTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(virtualAccount, expectedJsonString);

			string filepath2 = "dummy_virtual_account.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, url));
			virtualAccount = virtualAccount.Delete();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(virtualAccount, expectedJsonString);
		}

		[Fact]
		public void TestEmptyDeleteVirtualAccount()
		{
			string filepath = "dummy_virtual_account.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, virtualAccountURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.VirtualAccount.Delete());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}

		[Fact]
		public void TestGetAllPaymentsForVirtualAccount()
		{
			string filepath = "dummy_virtual_account.json";
			string url = string.Format("{0}/{1}", virtualAccountURL, VIRTUALACCOUNTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			VirtualAccount virtualAccount = client.VirtualAccount.Retrieve(VIRTUALACCOUNTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(virtualAccount, expectedJsonString);

			string filepath2 = "dummy_payment_collection.json";
			string payments_url = string.Format("{0}/payments", url);
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, payments_url));
			var result = virtualAccount.Payments();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestEmptyPaymentsForVirtualAccount()
		{
			string filepath = "dummy_virtual_account.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, virtualAccountURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.VirtualAccount.Payments());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}
	}
}
