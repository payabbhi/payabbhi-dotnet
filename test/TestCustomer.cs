using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestCustomer
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string CUSTOMERID = "dummy_customer_id";
		readonly string customerURL = "/api/v1/customers";

		[Fact]
		public void TestGetAllCustomers()
		{
			string filepath = "dummy_customer_collection.json";
      Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			var result = client.Customer.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllCustomersWithFilters()
		{
			string filepath = "dummy_customer_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", customerURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Customer.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetCustomerById()
		{
			string filepath = "dummy_customer.json";
			string url = string.Format("{0}/{1}", customerURL, CUSTOMERID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Customer customer = client.Customer.Retrieve(CUSTOMERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(customer, expectedJsonString);
		}

    [Fact]
		public void TestCreateCustomer()
		{
			string filepath = "dummy_customer.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("email", "b@b.com");
			options.Add("contact_no", "9433894351");
			Customer customer = client.Customer.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(customer, expectedJsonString);
		}

		[Fact]
		public void TestUpdateCustomer()
		{
			string filepath = "dummy_customer.json";
			string url = string.Format("{0}/{1}", customerURL, CUSTOMERID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Customer customer = client.Customer.Retrieve(CUSTOMERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(customer, expectedJsonString);

			string filepath2 = "dummy_customer_update.json";
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, url));
			customer = customer.Update(new Dictionary<string, object>() {
				{ "email", "a@b.com" },
				{ "contact_no", "8433894351" },
	  });
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertEntity(customer, expectedJsonString);
		}

		[Fact]
		public void TestEmptyCustomerUpdate()
		{
			string filepath = "dummy_customer_update.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Customer.Update(new Dictionary<string, object>() {
				{ "email", "a@b.com" },
				{ "contact_no", "8433894351" },
      }));
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}
	}
}
