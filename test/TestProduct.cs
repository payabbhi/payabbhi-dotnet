using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestProduct
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string PRODUCTID = "dummy_product_id";
		readonly string productURL = "/api/v1/products";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllProducts()
		{
			string filepath = "dummy_product_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, productURL));
			var result = client.Product.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfProducts(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllProductsWithFilters()
		{
			string filepath = "dummy_product_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", productURL, options["count"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Product.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfProducts(result, expectedJsonString);
		}

		[Test]
		public void TestGetProductById()
		{
			string filepath = "dummy_product.json";
			string url = string.Format("{0}/{1}", productURL, PRODUCTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Product product = client.Product.Retrieve(PRODUCTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertProduct(product, expectedJsonString);
		}

		[Test]
		public void TestCreateProduct()
		{
			string filepath = "dummy_product.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, productURL));
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("name", "Books");
			options.Add("unit_label", "MB");
			options.Add("notes", new Dictionary<string, object>(){
		{ "genre", "comedy" }
	  });
			Product product = client.Product.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertProduct(product, expectedJsonString);
		}
	}
}
