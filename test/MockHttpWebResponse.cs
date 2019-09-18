using System.IO;
using System.Net;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests {
    public class MockHttpWebResponse : IHttpWebResponse {
        HttpWebResponse _response;
        HttpStatusCode httpStatusCode;

        public HttpStatusCode StatusCode {
            get { return httpStatusCode; }
            set { httpStatusCode = value; }
        }

        public HttpWebResponse Response {
            get { return _response; }
            set {
                _response = value;
                httpStatusCode = _response.StatusCode;
            }
        }

        public virtual Stream GetResponseStream () {
            return _response.GetResponseStream ();
        }
    }
}