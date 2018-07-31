using System.Net;
namespace Payabbhi.Error
{
    public class GatewayError : BaseError
    {
        public GatewayError(string description, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode)
            :base(description, field, payabbhiResponse, httpStatusCode){}
    }
}
