using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestInvoiceItem
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string INVOICEITEMID = "dummy_invoice_item_id";
		readonly string invoiceItemURL = "/api/v1/invoiceitems";

		[Fact]
		public void TestGetAllInvoiceItems()
		{
			string filepath = "dummy_invoice_item_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceItemURL));
			var result = client.InvoiceItem.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllInvoiceItemsWithFilters()
		{
			string filepath = "dummy_invoice_item_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", invoiceItemURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.InvoiceItem.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetInvoiceItemById()
		{
			string filepath = "dummy_invoice_item.json";
			string url = string.Format("{0}/{1}", invoiceItemURL, INVOICEITEMID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			InvoiceItem invoiceItem = client.InvoiceItem.Retrieve(INVOICEITEMID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(invoiceItem, expectedJsonString);
		}

		[Fact]
		public void TestCreateInvoiceItem()
		{
			string filepath = "dummy_invoice_item.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceItemURL));
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("name", "Line Item");
			options.Add("customer_id", "cust_2WmsQoSRZMWWkcZg");
			options.Add("invoice_id", "invt_LN3GM0Ea7hVcsgr6");
			options.Add("amount", 100);
			options.Add("currency", "INR");
			InvoiceItem invoiceItem = client.InvoiceItem.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(invoiceItem, expectedJsonString);
		}

		[Fact]
		public void TestDeleteInvoiceItem()
		{
			string filepath = "dummy_invoice_item.json";
			string url = string.Format("{0}/{1}", invoiceItemURL, INVOICEITEMID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			InvoiceItem invoiceItem = client.InvoiceItem.Retrieve(INVOICEITEMID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(invoiceItem, expectedJsonString);

			string delete_url = string.Format("{0}/delete", url);
			client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, delete_url));
			invoiceItem = invoiceItem.Delete();
			expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(invoiceItem, expectedJsonString);
		}

		[Fact]
		public void TestEmptyDeleteInvoiceItem()
		{
			string filepath = "dummy_invoice_item.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceItemURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.InvoiceItem.Delete());
			Assert.Equal(ex.Message, "message: Object Id not set\n");
			Assert.Equal(ex.Description, Constants.Messages.InvalidCallError);
		}
	}
}
