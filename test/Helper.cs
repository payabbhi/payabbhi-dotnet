using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Payabbhi;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public static class Helper {
        public static Client client;

        public static MemoryStream LoadJson (string filepath) {
            string json = GetJsonString (filepath);
            var expectedBytes = Encoding.UTF8.GetBytes (json);
            var responseStream = new MemoryStream ();
            responseStream.Write (expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek (0, SeekOrigin.Begin);
            return responseStream;
        }

        public static string GetJsonString (string filepath) {
            var directory = string.Format ("{0}/{1}", Directory.GetCurrentDirectory (), @"../../testData/");
            var path = Path.Combine (directory, filepath);
            using (StreamReader r = new StreamReader (path)) {
                return r.ReadToEnd ();
            }
        }

        public static IHttpWebRequestFactory GetMockRequestFactory (string filepath, string url, HttpStatusCode statusCode = HttpStatusCode.OK) {
            var response = Substitute.For<MockHttpWebResponse> ();
            response.StatusCode = statusCode;
            var stream = Helper.LoadJson (filepath);
            response.GetResponseStream ().Returns (stream);

            var request = Substitute.For<MockHttpWebRequest> ();
            request.Request = (HttpWebRequest) WebRequest.Create (Client.BaseUrl + url);
            request.GetRequestStream ().Returns (new MemoryStream ());
            request.GetResponse ().Returns (response);

            var factory = Substitute.For<MockHttpWebRequestFactory> ();
            factory.Create (Arg.Any<string> ()).Returns (request);
            return factory;
        }

        public static IHttpWebRequestFactory GetErrorMockRequestFactory (string filepath, string url, HttpStatusCode statusCode = HttpStatusCode.OK) {
            var response = Substitute.For<MockHttpWebResponse> ();
            response.StatusCode = statusCode;
            var stream = Helper.LoadJson (filepath);
            response.GetResponseStream ().Returns (stream);

            WebResponse errorResponse = new WebException ().Response;
            var request = Substitute.For<MockHttpWebRequest> ();
            request.Request = (HttpWebRequest) WebRequest.Create (Client.BaseUrl + url);
            request.GetRequestStream ().Returns (new MemoryStream ());
            request.SetErrorResponse (errorResponse).Returns (response);
            request.GetResponse ().Returns (x => { throw new WebException (); });

            var factory = Substitute.For<MockHttpWebRequestFactory> ();
            factory.Create (Arg.Any<string> ()).Returns (request);
            return factory;
        }

        public static void AssertEntity (PayabbhiEntity entity, string expectedOutput) {
            JToken token = JObject.Parse (expectedOutput);
            Assert.NotSame (null, entity);
            JToken token2 = JToken.FromObject (entity);
            JToken.DeepEquals (token, token2);
            if (entity.PayabbhiResponse != null && entity.PayabbhiResponse.ResponseJson != null) {
                Assert.Equal (entity.PayabbhiResponse.ResponseJson, expectedOutput);
            }
        }
    }
}