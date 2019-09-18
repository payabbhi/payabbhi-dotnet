using System.Net;

namespace Payabbhi.Error {
    public class ApiConnectionError : BaseError {
        public ApiConnectionError (string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode = HttpStatusCode.Unused) : base (description, field, payabbhiResponse, httpStatusCode) { }
    }
}