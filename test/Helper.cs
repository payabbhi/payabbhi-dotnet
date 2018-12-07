using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	public static class Helper
	{
		public static Client client;

		public static MemoryStream LoadJson(string filepath)
		{
			string json = GetJsonString(filepath);
			var expectedBytes = Encoding.UTF8.GetBytes(json);
			var responseStream = new MemoryStream();
			responseStream.Write(expectedBytes, 0, expectedBytes.Length);
			responseStream.Seek(0, SeekOrigin.Begin);
			return responseStream;
		}

		public static string GetJsonString(string filepath)
		{
			var directory = string.Format("{0}/{1}", TestContext.CurrentContext.TestDirectory, @"../../testData/");
			var path = Path.Combine(directory, filepath);
			using (StreamReader r = new StreamReader(path))
			{
				return r.ReadToEnd();
			}
		}

		public static IHttpWebRequestFactory GetMockRequestFactory(string filepath, string url, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var response = Substitute.For<MockHttpWebResponse>();
			response.StatusCode = statusCode;
			var stream = Helper.LoadJson(filepath);
			response.GetResponseStream().Returns(stream);

			var request = Substitute.For<MockHttpWebRequest>();
			request.Request = (HttpWebRequest)WebRequest.Create(Client.BaseUrl + url);
			request.GetRequestStream().Returns(new MemoryStream());
			request.GetResponse().Returns(response);

			var factory = Substitute.For<MockHttpWebRequestFactory>();
			factory.Create(Arg.Any<string>()).Returns(request);
			return factory;
		}

		public static IHttpWebRequestFactory GetErrorMockRequestFactory(string filepath, string url, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			var response = Substitute.For<MockHttpWebResponse>();
			response.StatusCode = statusCode;
			var stream = Helper.LoadJson(filepath);
			response.GetResponseStream().Returns(stream);

			WebResponse errorResponse = new WebException().Response;
			var request = Substitute.For<MockHttpWebRequest>();
			request.Request = (HttpWebRequest)WebRequest.Create(Client.BaseUrl + url);
			request.GetRequestStream().Returns(new MemoryStream());
			request.SetErrorResponse(errorResponse).Returns(response);
			request.GetResponse().Returns(x => { throw new WebException(); });

			var factory = Substitute.For<MockHttpWebRequestFactory>();
			factory.Create(Arg.Any<string>()).Returns(request);
			return factory;
		}

		public static void AssertOrder(Order order, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, order);
			if (validateJson)
			{
				Assert.AreEqual(order.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(order.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(order.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(order.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(order.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(order.Status, (string)token.SelectToken("status"));
			Assert.AreEqual(order.CreatedAt, (int)token.SelectToken("created_at"));
			Assert.AreEqual(order.Notes, token.SelectToken("notes"));
			Assert.AreEqual(order.MerchantOrderId, (string)token.SelectToken("merchant_order_id"));
			Assert.AreEqual(order.PaymentAttempts, (int)token.SelectToken("payment_attempts"));
		}

		public static void AssertPayment(Payment payment, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, payment);
			if (validateJson)
			{
				Assert.AreEqual(payment.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(payment.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(payment.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(payment.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(payment.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(payment.Status, (string)token.SelectToken("status"));
			Assert.AreEqual(payment.Fee, (int)token.SelectToken("fee"));
			Assert.AreEqual(payment.Description, (string)token.SelectToken("description"));
			Assert.AreEqual(payment.CreatedAt, (int)token.SelectToken("created_at"));
			Assert.AreEqual(payment.Bank, (string)token.SelectToken("bank"));
			Assert.AreEqual(payment.Card, (string)token.SelectToken("card"));
			Assert.AreEqual(payment.Contact, (string)token.SelectToken("contact"));
			Assert.AreEqual(payment.Email, (string)token.SelectToken("email"));
			Assert.AreEqual(payment.ErrorCode, (string)token.SelectToken("error_code"));
			Assert.AreEqual(payment.ErrorDescription, (string)token.SelectToken("error_description"));
			Assert.AreEqual(payment.International, (bool)token.SelectToken("international"));
			Assert.AreEqual(payment.Method, (string)token.SelectToken("method"));
			Assert.AreEqual(payment.Notes, (object)token.SelectToken("notes"));
			Assert.AreEqual(payment.OrderId, (string)token.SelectToken("order_id"));
			Assert.AreEqual(payment.PayoutAmount, (int)token.SelectToken("payout_amount"));
			Assert.AreEqual(payment.PayoutType, (string)token.SelectToken("payout_type"));
			Assert.AreEqual(payment.RefundStatus, (string)token.SelectToken("refund_status"));
			Assert.AreEqual(payment.RefundedAmount, (int)token.SelectToken("refunded_amount"));
			Assert.AreEqual(payment.ServiceTax, (int)token.SelectToken("service_tax"));
			Assert.AreEqual(payment.Wallet, (string)token.SelectToken("wallet"));
			Assert.AreEqual(payment.VPA, (string)token.SelectToken("vpa"));
			if (payment.Emi != null)
			{
				AssertEmi(payment.Emi, token.SelectToken("emi").ToString(Formatting.None));
			}
			else
			{
				Assert.AreEqual(payment.Emi, (string)token.SelectToken("emi"));
			}
			AssertListOfRefunds(payment.Refunds, token.SelectToken("refunds").ToString(Formatting.None));
		}

		public static void AssertEmi(EmiInfo emi, string expectedOutput)
		{
			Assert.AreNotSame(null, emi);
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreEqual(emi.Tenure, (int)token.SelectToken("tenure"));
			Assert.AreEqual(emi.InterestRate, (int)token.SelectToken("interest_rate"));
			Assert.AreEqual(emi.Provider, (string)token.SelectToken("provider"));
			Assert.AreEqual(emi.Subvention, (string)token.SelectToken("subvention"));
		}

		public static void AssertRefund(Refund refund, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, refund);
			if (validateJson)
			{
				Assert.AreEqual(refund.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(refund.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(refund.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(refund.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(refund.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(refund.CreatedAt, (int)token.SelectToken("created_at"));
			Assert.AreEqual(refund.Notes, (object)token.SelectToken("notes"));
			Assert.AreEqual(refund.PaymentId, (string)token.SelectToken("payment_id"));
		}

		public static void AssertProduct(Product product, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, product);
			if (validateJson)
			{
				Assert.AreEqual(product.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(product.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(product.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(product.Name, (string)token.SelectToken("name"));
			Assert.AreEqual(product.Type, (string)token.SelectToken("type"));
			Assert.AreEqual(product.UnitLabel, (string)token.SelectToken("unit_label"));
			if (product.Notes != null)
			{
				Assert.AreEqual(product.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(product.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertPlan(Plan plan, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, plan);
			if (validateJson)
			{
				Assert.AreEqual(plan.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(plan.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(plan.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(plan.ProductId, (string)token.SelectToken("product_id"));
			Assert.AreEqual(plan.Name, (string)token.SelectToken("name"));
			Assert.AreEqual(plan.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(plan.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(plan.Frequency, (int)token.SelectToken("frequency"));
			Assert.AreEqual(plan.Interval, (string)token.SelectToken("interval"));
			if (plan.Notes != null)
			{
				Assert.AreEqual(plan.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(plan.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertListOfRefunds(PayabbhiList<Refund> refundList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, refundList);
			Assert.AreEqual(refundList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(refundList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedRefunds = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Refund refund in refundList)
			{
				string item = expectedRefunds[count++].ToString(Formatting.None);
				AssertRefund(refund, item, false);
			}
		}

		public static void AssertListOfPayments(PayabbhiList<Payment> paymentList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, paymentList);
			Assert.AreEqual(paymentList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(paymentList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedPayments = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Payment payment in paymentList)
			{
				string item = expectedPayments[count++].ToString(Formatting.None);
				AssertPayment(payment, item, false);
			}
		}

		public static void AssertListOfOrders(PayabbhiList<Order> orderList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, orderList);
			Assert.AreEqual(orderList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(orderList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedOrders = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Order order in orderList)
			{
				string item = expectedOrders[count++].ToString(Formatting.None);
				AssertOrder(order, item, false);
			}
		}

		public static void AssertListOfProducts(PayabbhiList<Product> productList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, productList);
			Assert.AreEqual(productList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(productList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedProducts = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Product product in productList)
			{
				string item = expectedProducts[count++].ToString(Formatting.None);
				AssertProduct(product, item, false);
			}
		}

		public static void AssertListOfPlans(PayabbhiList<Plan> planList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, planList);
			Assert.AreEqual(planList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(planList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedPlans = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Plan plan in planList)
			{
				string item = expectedPlans[count++].ToString(Formatting.None);
				AssertPlan(plan, item, false);
			}
		}

		public static void AssertListOfCustomers(PayabbhiList<Customer> customerList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, customerList);
			Assert.AreEqual(customerList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(customerList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedCustomers = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Customer customer in customerList)
			{
				string item = expectedCustomers[count++].ToString(Formatting.None);
				AssertCustomer(customer, item, false);
			}
		}

		public static void AssertCustomer(Customer customer, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, customer);
			if (validateJson)
			{
				Assert.AreEqual(customer.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(customer.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(customer.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(customer.Name, (string)token.SelectToken("name"));
			Assert.AreEqual(customer.ContactNo, (string)token.SelectToken("contact_no"));
			Assert.AreEqual(customer.Email, (string)token.SelectToken("email"));
			if (customer.BillingAddress != null)
			{
				Assert.AreEqual(customer.BillingAddress, (object)token.SelectToken("billing_address"));
			}
			if (customer.ShippingAddress != null)
			{
				Assert.AreEqual(customer.ShippingAddress, (object)token.SelectToken("shipping_address"));
			}
			Assert.AreEqual(customer.Gstin, (string)token.SelectToken("gstin"));
			if (customer.Notes != null)
			{
				Assert.AreEqual(customer.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(customer.CreatedAt, (int)token.SelectToken("created_at"));
			AssertListOfSubscriptions(customer.Subscriptions, token.SelectToken("subscriptions").ToString(Formatting.None));
		}

		public static void AssertListOfSubscriptions(PayabbhiList<Subscription> subscriptionList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, subscriptionList);
			Assert.AreEqual(subscriptionList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(subscriptionList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedSubscriptions = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Subscription subscription in subscriptionList)
			{
				string item = expectedSubscriptions[count++].ToString(Formatting.None);
				AssertSubscription(subscription, item, false);
			}
		}

		public static void AssertSubscription(Subscription subscription, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, subscription);
			if (validateJson)
			{
				Assert.AreEqual(subscription.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(subscription.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(subscription.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(subscription.PlanId, (string)token.SelectToken("plan_id"));
			Assert.AreEqual(subscription.CustomerId, (string)token.SelectToken("customer_id"));
			Assert.AreEqual(subscription.BillingMethod, (string)token.SelectToken("billing_method"));
			Assert.AreEqual(subscription.Quantity, (int)token.SelectToken("quantity"));
			Assert.AreEqual(subscription.CustomerNotificationBy, (string)token.SelectToken("customer_notification_by"));
			Assert.AreEqual(subscription.BillingCycleCount, (int)token.SelectToken("billing_cycle_count"));
			Assert.AreEqual(subscription.PaidCount, (int)token.SelectToken("paid_count"));
			Assert.AreEqual(subscription.CancelAtPeriodEnd, (bool)token.SelectToken("cancel_at_period_end"));
			Assert.AreEqual(subscription.DueByDays, (int)token.SelectToken("due_by_days"));
			Assert.AreEqual(subscription.TrialEndAt, (int)token.SelectToken("trial_end_at"));
			Assert.AreEqual(subscription.Status, (string)token.SelectToken("status"));
			Assert.AreEqual(subscription.CurrentStartAt, (int)token.SelectToken("current_start_at"));
			Assert.AreEqual(subscription.CurrentEndAt, (int)token.SelectToken("current_end_at"));
			Assert.AreEqual(subscription.EndedAt, (int)token.SelectToken("ended_at"));
			Assert.AreEqual(subscription.CancelledAt, (int)token.SelectToken("cancelled_at"));
			if (subscription.Notes != null)
			{
				Assert.AreEqual(subscription.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(subscription.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertListOfInvoiceItems(PayabbhiList<InvoiceItem> invoiceItemsList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, invoiceItemsList);
			Assert.AreEqual(invoiceItemsList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(invoiceItemsList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedInvoiceItems = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (InvoiceItem invoiceItem in invoiceItemsList)
			{
				string item = expectedInvoiceItems[count++].ToString(Formatting.None);
				AssertInvoiceItem(invoiceItem, item, false);
			}
		}

		public static void AssertInvoiceItem(InvoiceItem invoiceItem, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, invoiceItem);
			if (validateJson)
			{
				Assert.AreEqual(invoiceItem.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(invoiceItem.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(invoiceItem.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(invoiceItem.Name, (string)token.SelectToken("name"));
			Assert.AreEqual(invoiceItem.Description, (string)token.SelectToken("description"));
			Assert.AreEqual(invoiceItem.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(invoiceItem.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(invoiceItem.CustomerId, (string)token.SelectToken("customer_id"));
			Assert.AreEqual(invoiceItem.InvoiceId, (string)token.SelectToken("invoice_id"));
			Assert.AreEqual(invoiceItem.SubscriptionId, (string)token.SelectToken("subscription_id"));
			Assert.AreEqual(invoiceItem.Quantity, (int)token.SelectToken("quantity"));
			if (invoiceItem.Notes != null)
			{
				Assert.AreEqual(invoiceItem.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(invoiceItem.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertListOfInvoices(PayabbhiList<Invoice> invoicesList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, invoicesList);
			Assert.AreEqual(invoicesList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(invoicesList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedInvoices = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Invoice invoice in invoicesList)
			{
				string item = expectedInvoices[count++].ToString(Formatting.None);
				AssertInvoice(invoice, item, false);
			}
		}

		public static void AssertInvoice(Invoice invoice, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, invoice);
			if (validateJson)
			{
				Assert.AreEqual(invoice.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(invoice.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(invoice.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(invoice.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(invoice.BillingMethod, (string)token.SelectToken("billing_method"));
			Assert.AreEqual(invoice.CustomerId, (string)token.SelectToken("customer_id"));
			Assert.AreEqual(invoice.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(invoice.Description, (string)token.SelectToken("description"));
			Assert.AreEqual(invoice.DueDate, (int)token.SelectToken("due_date"));
			Assert.AreEqual(invoice.NotifyBy, (string)token.SelectToken("notify_by"));
			Assert.AreEqual(invoice.PaymentAttempt, (int)token.SelectToken("payment_attempt"));
			Assert.AreEqual(invoice.InvoiceNo, (string)token.SelectToken("invoice_no"));
			Assert.AreEqual(invoice.Status, (string)token.SelectToken("status"));
			Assert.AreEqual(invoice.SubscriptionId, (string)token.SelectToken("subscription_id"));
			Assert.AreEqual(invoice.Url, (string)token.SelectToken("url"));
			if (invoice.Notes != null)
			{
				Assert.AreEqual(invoice.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(invoice.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertListOfTransfers(PayabbhiList<Transfer> transfersList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, transfersList);
			Assert.AreEqual(transfersList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(transfersList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedTransfers = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Transfer transfer in transfersList)
			{
				string item = expectedTransfers[count++].ToString(Formatting.None);
				AssertTransfer(transfer, item, false);
			}
		}

		public static void AssertTransfer(Transfer transfer, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, transfer);
			if (validateJson)
			{
				Assert.AreEqual(transfer.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(transfer.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(transfer.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(transfer.Description, (string)token.SelectToken("description"));
			Assert.AreEqual(transfer.SourceId, (string)token.SelectToken("source_id"));
			Assert.AreEqual(transfer.RecipientId, (string)token.SelectToken("recipient_id"));
			Assert.AreEqual(transfer.Amount, (int)token.SelectToken("amount"));
			Assert.AreEqual(transfer.Currency, (string)token.SelectToken("currency"));
			Assert.AreEqual(transfer.Fees, (int)token.SelectToken("fees"));
			Assert.AreEqual(transfer.Gst, (int)token.SelectToken("gst"));
			if (transfer.Notes != null)
			{
				Assert.AreEqual(transfer.Notes, (object)token.SelectToken("notes"));
			}
			Assert.AreEqual(transfer.CreatedAt, (int)token.SelectToken("created_at"));
		}

		public static void AssertListOfEvents(PayabbhiList<Event> eventsList, string expectedOutput)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, eventsList);
			Assert.AreEqual(eventsList.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(eventsList.TotalCount, (int)token.SelectToken("total_count"));
			JArray expectedEvents = (JArray)token.SelectToken("data");
			int count = 0;
			foreach (Event evt in eventsList)
			{
				string item = expectedEvents[count++].ToString(Formatting.None);
				AssertEvent(evt, item, false);
			}
		}

		public static void AssertEvent(Event evt, string expectedOutput, bool validateJson = true)
		{
			JToken token = JObject.Parse(expectedOutput);
			Assert.AreNotSame(null, evt);
			if (validateJson)
			{
				Assert.AreEqual(evt.PayabbhiResponse.ResponseJson, expectedOutput);
			}
			Assert.AreEqual(evt.Id, (string)token.SelectToken("id"));
			Assert.AreEqual(evt.Object, (string)token.SelectToken("object"));
			Assert.AreEqual(evt.Type, (string)token.SelectToken("type"));
			Assert.AreEqual(evt.Environment, (string)token.SelectToken("environment"));
			if (evt.Data != null)
			{
				Assert.AreEqual(evt.Data, (object)token.SelectToken("data"));
			}
			Assert.AreEqual(evt.CreatedAt, (int)token.SelectToken("created_at"));
		}

	}
}
