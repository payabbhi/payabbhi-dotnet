using System.Net;

namespace Payabbhi.Error {
    public class SignatureVerificationError : BaseError {
        public SignatureVerificationError (string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode = HttpStatusCode.Unused) : base (description, field, payabbhiResponse, httpStatusCode) { }
    }
}