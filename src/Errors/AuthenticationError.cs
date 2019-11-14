using System.Net;
namespace Payabbhi.Error {
    public class AuthenticationError : BaseError {
        public AuthenticationError (string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode) : base (description, field, payabbhiResponse, httpStatusCode) { }
    }
}