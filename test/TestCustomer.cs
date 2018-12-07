using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestCustomer
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string CUSTOMERID = "dummy_customer_id";
		readonly string customerURL = "/api/v1/customers";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllCustomers()
		{
			string filepath = "dummy_customer_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			var result = client.Customer.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfCustomers(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllCustomersWithFilters()
		{
			string filepath = "dummy_customer_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", customerURL, options["count"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Customer.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfCustomers(result, expectedJsonString);
		}

		[Test]
		public void TestGetCustomerById()
		{
			string filepath = "dummy_customer.json";
			string url = string.Format("{0}/{1}", customerURL, CUSTOMERID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Customer customer = client.Customer.Retrieve(CUSTOMERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertCustomer(customer, expectedJsonString);
		}

		[Test]
		public void TestCreateCustomer()
		{
			string filepath = "dummy_customer.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("email", "b@b.com");
			options.Add("contact_no", "9433894351");
			Customer customer = client.Customer.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertCustomer(customer, expectedJsonString);
		}

		[Test]
		public void TestUpdateCustomer()
		{
			string filepath = "dummy_customer.json";
			string url = string.Format("{0}/{1}", customerURL, CUSTOMERID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Customer customer = client.Customer.Retrieve(CUSTOMERID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertCustomer(customer, expectedJsonString);

			string filepath2 = "dummy_customer_update.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, url));
			customer = customer.Update(new Dictionary<string, object>() {
				{ "email", "a@b.com" },
				{ "contact_no", "8433894351" },
	  });
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertCustomer(customer, expectedJsonString);
		}

		[Test]
		public void TestEmptyCustomerUpdate()
		{
			string filepath = "dummy_customer_update.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, customerURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Customer.Update(new Dictionary<string, object>() {
				{ "email", "a@b.com" },
				{ "contact_no", "8433894351" },
	  }));
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}
	}
}
