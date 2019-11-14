using System.Net;
using NSubstitute;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestError {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string PAYMENTID = "dummy_payment_id";
        readonly string paymentURL = "/api/v1/payments";

        [Fact]
        public void TestHandleJsonDeserializationError () {
            string filepath = "dummy_invalid_data.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.InternalServerError));
            var ex = Assert.Throws<ApiError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Something did not work as expected on our side, httpStatusCode: 500\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.InternalServerError);
            Assert.Equal (ex.Description, Constants.Messages.ApiError);
            Assert.Equal (ex.Field, null);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
        }

        [Fact]
        public void TestHandleJsonDeserializationError2 () {
            string filepath = "dummy_invalid_json.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.InternalServerError));
            var ex = Assert.Throws<ApiError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Something did not work as expected on our side, httpStatusCode: 500\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.InternalServerError);
            Assert.Equal (ex.Description, Constants.Messages.ApiError);
            Assert.Equal (ex.Field, null);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
        }

        [Fact]
        public void TestHandleInvalidRequestError () {
            string filepath = "dummy_invalid_request.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.BadRequest));
            var ex = Assert.Throws<InvalidRequestError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: An invalid value was specified for one of the request parameters in the URL, httpStatusCode: 400, field: count\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.BadRequest);
            Assert.Equal (ex.Description, "An invalid value was specified for one of the request parameters in the URL");
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, "count");
        }

        [Fact]
        public void TestHandleInvalidRequestError2 () {
            string filepath = "dummy_invalid_request.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.BadRequest));
            var ex = Assert.Throws<InvalidRequestError> (() => client.Payment.Capture ());
            Assert.Equal (ex.Message, "message: Object Id not set\n");
            Assert.Equal (ex.PayabbhiResponse, null);
            Assert.Equal (ex.Field, null);
            Assert.Equal (ex.Description, "Object Id not set");
        }

        [Fact]
        public void TestHandleAuthenticationError () {
            string filepath = "dummy_authentication.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.Unauthorized));
            var ex = Assert.Throws<AuthenticationError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Incorrect access_id or secret_key provided., httpStatusCode: 401\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.Unauthorized);
            Assert.Equal (ex.Description, "Incorrect access_id or secret_key provided.");
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, string.Empty);
        }

        [Fact]
        public void TestHandleApiError () {
            string filepath = "dummy_server_error.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.InternalServerError));
            var ex = Assert.Throws<ApiError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: There is some problem with the server, httpStatusCode: 500, field: 500\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.InternalServerError);
            Assert.Equal (ex.Description, "There is some problem with the server");
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, "500");
        }

        [Fact]
        public void TestHandleGatewayErrorWithEmptyMessage () {
            string filepath = "dummy_http_code_502_with_empty_message.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.BadGateway));
            var ex = Assert.Throws<ApiError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Something did not work as expected on our side, httpStatusCode: 502\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.BadGateway);
            Assert.Equal (ex.Description, Constants.Messages.ApiError);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, null);
        }

        [Fact]
        public void TestHandleGatewayError () {
            string filepath = "dummy_gateway_error.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.BadGateway));
            var ex = Assert.Throws<GatewayError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Unable to create refund. Please try again., httpStatusCode: 502, field: 502\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.BadGateway);
            Assert.Equal (ex.Description, "Unable to create refund. Please try again.");
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, "502");
        }

        [Fact]
        public void TestHandleStatusNotFoundError () {
            string filepath = "dummy_status_not_found.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetErrorMockRequestFactory (filepath, paymentURL, HttpStatusCode.Ambiguous));
            var ex = Assert.Throws<ApiError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Unexpected HTTP code: MultipleChoices, httpStatusCode: 300\n");
            Assert.Equal (ex.HttpStatusCode, HttpStatusCode.Ambiguous);
            Assert.Equal (ex.Description, "Unexpected HTTP code: MultipleChoices");
            string expectedJsonString = Helper.GetJsonString (filepath);
            Assert.Equal (ex.PayabbhiResponse.ResponseJson, expectedJsonString);
            Assert.Equal (ex.Field, null);
        }

        [Fact]
        public void TestHandleNullResponseError () {
            var request = Substitute.For<MockHttpWebRequest> ();
            request.Request = (HttpWebRequest) WebRequest.Create (Client.BaseUrl + paymentURL);
            request.GetResponse ().Returns ((IHttpWebResponse) null);

            var factory = Substitute.For<MockHttpWebRequestFactory> ();
            factory.Create (Arg.Any<string> ()).Returns (request);
            Client client = new Client (ACCESSID, SECRETKEY, factory);
            var ex = Assert.Throws<ApiConnectionError> (() => client.Payment.All ());
            Assert.Equal (ex.Message, "message: Unexpected error communicating with Payabbhi. If this problem persists, let us know at support@payabbhi.com.\n");
            Assert.Equal (ex.Description, Constants.Messages.ApiConnectionError);
            Assert.Equal (ex.Field, null);
            Assert.Equal (ex.PayabbhiResponse, null);
        }

        [Fact]
        public void TestHandleRequestWebExceptionError () {
            string url = string.Format ("{0}/{1}/refunds", paymentURL, PAYMENTID);
            var request = Substitute.For<MockHttpWebRequest> ();
            request.Request = (HttpWebRequest) WebRequest.Create (Client.BaseUrl + url);
            request.GetRequestStream ().Returns (x => { throw new WebException (); });

            var factory = Substitute.For<MockHttpWebRequestFactory> ();
            factory.Create (Arg.Any<string> ()).Returns (request);
            Client client = new Client (ACCESSID, SECRETKEY, factory);
            var ex = Assert.Throws<ApiConnectionError> (() => client.Refund.Create ("dummy_refund_id"));
            Assert.Equal (ex.Message, "message: Unexpected error communicating with Payabbhi. If this problem persists, let us know at support@payabbhi.com.\n");
            Assert.Equal (ex.Description, Constants.Messages.ApiConnectionError);
            Assert.Equal (ex.Field, null);
            Assert.Equal (ex.PayabbhiResponse, null);
        }
    }
}