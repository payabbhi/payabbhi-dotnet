using System.Net;
using NSubstitute;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestError
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string PAYMENTID = "dummy_payment_id";
		readonly string paymentURL = "/api/v1/payments";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestHandleJsonDeserializationError()
		{
			string filepath = "dummy_invalid_data.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.InternalServerError));
			var ex = Assert.Throws<ApiError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Something did not work as expected on our side, httpStatusCode: 500\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.ApiError));
			Assert.That(ex.Field, Is.EqualTo(null));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
		}

		[Test]
		public void TestHandleJsonDeserializationError2()
		{
			string filepath = "dummy_invalid_json.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.InternalServerError));
			var ex = Assert.Throws<ApiError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Something did not work as expected on our side, httpStatusCode: 500\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.ApiError));
			Assert.That(ex.Field, Is.EqualTo(null));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
		}

		[Test]
		public void TestHandleInvalidRequestError()
		{
			string filepath = "dummy_invalid_request.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.BadRequest));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: An invalid value was specified for one of the request parameters in the URL, httpStatusCode: 400, field: count\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(ex.Description, Is.EqualTo("An invalid value was specified for one of the request parameters in the URL"));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo("count"));
		}

		[Test]
		public void TestHandleInvalidRequestError2()
		{
			string filepath = "dummy_invalid_request.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.BadRequest));
			var ex = Assert.Throws<InvalidRequestError>(() => client.Payment.Capture());
			Assert.That(ex.Message, Is.EqualTo("message: Object Id not set\n"));
			Assert.That(ex.PayabbhiResponse, Is.EqualTo(null));
			Assert.That(ex.Field, Is.EqualTo(null));
			Assert.That(ex.Description, Is.EqualTo("Object Id not set"));
		}

		[Test]
		public void TestHandleAuthenticationError()
		{
			string filepath = "dummy_authentication.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.Unauthorized));
			var ex = Assert.Throws<AuthenticationError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Incorrect access_id or secret_key provided., httpStatusCode: 401\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
			Assert.That(ex.Description, Is.EqualTo("Incorrect access_id or secret_key provided."));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo(string.Empty));
		}

		[Test]
		public void TestHandleApiError()
		{
			string filepath = "dummy_server_error.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.InternalServerError));
			var ex = Assert.Throws<ApiError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: There is some problem with the server, httpStatusCode: 500, field: 500\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
			Assert.That(ex.Description, Is.EqualTo("There is some problem with the server"));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo("500"));
		}

		[Test]
		public void TestHandleGatewayErrorWithEmptyMessage()
		{
			string filepath = "dummy_http_code_502_with_empty_message.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.BadGateway));
			var ex = Assert.Throws<ApiError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Something did not work as expected on our side, httpStatusCode: 502\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadGateway));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.ApiError));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo(null));
		}

		[Test]
		public void TestHandleGatewayError()
		{
			string filepath = "dummy_gateway_error.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.BadGateway));
			var ex = Assert.Throws<GatewayError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Unable to create refund. Please try again., httpStatusCode: 502, field: 502\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadGateway));
			Assert.That(ex.Description, Is.EqualTo("Unable to create refund. Please try again."));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo("502"));
		}

		[Test]
		public void TestHandleStatusNotFoundError()
		{
			string filepath = "dummy_status_not_found.json";
			Init(ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory(filepath, paymentURL, HttpStatusCode.Ambiguous));
			var ex = Assert.Throws<ApiError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Unexpected HTTP code: MultipleChoices, httpStatusCode: 300\n"));
			Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.Ambiguous));
			Assert.That(ex.Description, Is.EqualTo("Unexpected HTTP code: MultipleChoices"));
			string expectedJsonString = Helper.GetJsonString(filepath);
			Assert.That(ex.PayabbhiResponse.ResponseJson, Is.EqualTo(expectedJsonString));
			Assert.That(ex.Field, Is.EqualTo(null));
		}

		[Test]
		public void TestHandleNullResponseError()
		{
			var request = Substitute.For<MockHttpWebRequest>();
			request.Request = (HttpWebRequest)WebRequest.Create(Client.BaseUrl + paymentURL);
			request.GetResponse().Returns((IHttpWebResponse)null);

			var factory = Substitute.For<MockHttpWebRequestFactory>();
			factory.Create(Arg.Any<string>()).Returns(request);
			Init(ACCESSID, SECRETKEY, factory);
			var ex = Assert.Throws<ApiConnectionError>(() => client.Payment.All());
			Assert.That(ex.Message, Is.EqualTo("message: Unexpected error communicating with Payabbhi. If this problem persists, let us know at support@payabbhi.com.\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.ApiConnectionError));
			Assert.That(ex.Field, Is.EqualTo(null));
			Assert.That(ex.PayabbhiResponse, Is.EqualTo(null));
		}

		[Test]
		public void TestHandleRequestWebExceptionError()
		{
			string url = string.Format("{0}/{1}/refunds", paymentURL, PAYMENTID);
			var request = Substitute.For<MockHttpWebRequest>();
			request.Request = (HttpWebRequest)WebRequest.Create(Client.BaseUrl + url);
			request.GetRequestStream().Returns(x => { throw new WebException(); });

			var factory = Substitute.For<MockHttpWebRequestFactory>();
			factory.Create(Arg.Any<string>()).Returns(request);
			Init(ACCESSID, SECRETKEY, factory);
			var ex = Assert.Throws<ApiConnectionError>(() => client.Refund.Create("dummy_refund_id"));
			Assert.That(ex.Message, Is.EqualTo("message: Unexpected error communicating with Payabbhi. If this problem persists, let us know at support@payabbhi.com.\n"));
			Assert.That(ex.Description, Is.EqualTo(Constants.Messages.ApiConnectionError));
			Assert.That(ex.Field, Is.EqualTo(null));
			Assert.That(ex.PayabbhiResponse, Is.EqualTo(null));
		}
	}
}
