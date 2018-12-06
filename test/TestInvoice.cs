using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestInvoice
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string INVOICEID = "dummy_invoice_id";
		readonly string invoiceURL = "/api/v1/invoices";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllInvoices()
		{
			string filepath = "dummy_invoice_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceURL));
			var result = client.Invoice.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfInvoices(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllInvoicesWithFilters()
		{
			string filepath = "dummy_invoice_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", invoiceURL, options["count"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Invoice.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfInvoices(result, expectedJsonString);
		}

		[Test]
		public void TestGetInvoiceById()
		{
			string filepath = "dummy_invoice.json";
			string url = string.Format("{0}/{1}", invoiceURL, INVOICEID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Invoice invoice = client.Invoice.Retrieve(INVOICEID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertInvoice(invoice, expectedJsonString);
		}

    [Test]
		public void TestCreateInvoice()
		{
			string filepath = "dummy_invoice.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceURL));
			Dictionary<string,string> invItem = new Dictionary<string, string>() {
      	{"id", "item_I9Gh0wJHJ2tvorbT" }
      };
      Dictionary<string, string>[] invItemArr = { invItem };
			IDictionary<string, object> options = new Dictionary<string, object>();
			options.Add("invoice_no", "INV_90976164");
			options.Add("customer_id", "cust_2WmsQoSRZMWWkcZg");
			options.Add("due_date", 1539171804);
			options.Add("currency", "INR");
			options.Add("line_items", invItemArr);
			Invoice invoice = client.Invoice.Create(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertInvoice(invoice, expectedJsonString);
		}

		[Test]
		public void TestVoidInvoice()
		{
			string filepath = "dummy_invoice.json";
			string url = string.Format("{0}/{1}", invoiceURL, INVOICEID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Invoice invoice = client.Invoice.Retrieve(INVOICEID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertInvoice(invoice, expectedJsonString);

	  	string filepath2 = "dummy_invoice_void.json";
			string void_url = string.Format("{0}/void", url);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, void_url));
			invoice = invoice.Void();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertInvoice(invoice, expectedJsonString);
		}

		[Test]
		public void TestEmptyVoidInvoice()
		{
			string filepath = "dummy_invoice.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Invoice.Void());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestGetAllLineItemsForInvoice()
		{
			string filepath = "dummy_invoice.json";
			string url = string.Format("{0}/{1}", invoiceURL, INVOICEID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Invoice invoice = client.Invoice.Retrieve(INVOICEID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertInvoice(invoice, expectedJsonString);

			string filepath2 = "dummy_invoice_item_collection.json";
			string line_items_url = string.Format("{0}/line_items", url);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, line_items_url));
			var result = invoice.LineItems();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertListOfInvoiceItems(result, expectedJsonString);
		}

		[Test]
		public void TestEmptyLineItemsForInvoice()
		{
			string filepath = "dummy_invoice.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Invoice.LineItems());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}

		[Test]
		public void TestGetAllPaymentsForInvoice()
		{
			string filepath = "dummy_invoice.json";
			string url = string.Format("{0}/{1}", invoiceURL, INVOICEID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Invoice invoice = client.Invoice.Retrieve(INVOICEID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertInvoice(invoice, expectedJsonString);

			string filepath2 = "dummy_payment_collection.json";
			string payments_url = string.Format("{0}/payments", url);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath2, payments_url));
			var result = invoice.Payments();
			expectedJsonString = Helper.GetJsonString(filepath2);
			Helper.AssertListOfPayments(result, expectedJsonString);
		}

		[Test]
		public void TestEmptyPaymentsForInvoice()
		{
			string filepath = "dummy_invoice.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, invoiceURL));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Invoice.Payments());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.InvalidCallError));
		}
	}
}
