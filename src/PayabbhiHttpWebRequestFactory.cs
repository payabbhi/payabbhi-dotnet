using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Payabbhi {
    [ExcludeFromCodeCoverage]
    public class PayabbhiHttpWebRequestFactory : IHttpWebRequestFactory {
        public IHttpWebRequest Create (string uri) {
            return new PayabbhiHttpWebRequest ((HttpWebRequest) WebRequest.Create (uri));
        }
    }
}