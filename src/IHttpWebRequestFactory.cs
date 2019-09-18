using System;
namespace Payabbhi {
    public interface IHttpWebRequestFactory {
        IHttpWebRequest Create (string uri);
    }
}