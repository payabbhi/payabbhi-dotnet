using System.IO;
using System.Net;

namespace Payabbhi {
    public interface IHttpWebRequest {
        string Method { get; set; }
        bool KeepAlive { get; set; }
        string ContentType { get; set; }
        WebHeaderCollection Headers { get; set; }
        string UserAgent { get; set; }

        IHttpWebResponse GetResponse ();
        IHttpWebResponse SetErrorResponse (WebResponse response);
        Stream GetRequestStream ();
    }
}