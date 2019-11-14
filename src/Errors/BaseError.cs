using System;
using System.Net;

namespace Payabbhi.Error {
    public abstract class BaseError : Exception {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
        public PayabbhiResponse PayabbhiResponse { get; set; }
        static string ErrorMessage (string description, string field, HttpStatusCode httpStatusCode) {
            string message = "message: " + description;
            if (httpStatusCode != HttpStatusCode.Unused) {
                message += ", httpStatusCode: " + (int) httpStatusCode;
            }
            if (!string.IsNullOrWhiteSpace (field)) {
                message += ", field: " + field;
            }
            return message + "\n";
        }

        protected BaseError (string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode = HttpStatusCode.Unused) : base (ErrorMessage (description, field, httpStatusCode)) {
            this.HttpStatusCode = httpStatusCode;
            this.Description = description;
            this.Field = field;
            this.PayabbhiResponse = payabbhiResponse;
        }
    }
}