using System.Net;
namespace Payabbhi.Error
{
    public class InvalidRequestError : BaseError
    {
        public InvalidRequestError(string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode)
            :base(description, field, payabbhiResponse, httpStatusCode){}
    }
}
