using System;
using System.IO;
using System.Net;

namespace Payabbhi
{
	public interface IHttpWebResponse
	{
        HttpStatusCode StatusCode { get; set; }
        Stream GetResponseStream();
    }
}
