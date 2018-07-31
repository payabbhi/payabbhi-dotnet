using System.Net;
namespace Payabbhi.Error
{
    public class ApiError : BaseError
    {
        public ApiError(string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode)
            :base(description, field, payabbhiResponse, httpStatusCode){}
    }
}
