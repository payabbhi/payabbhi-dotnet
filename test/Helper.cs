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
	}
}
