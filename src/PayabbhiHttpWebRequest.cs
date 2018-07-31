using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace Payabbhi
{
	[ExcludeFromCodeCoverage]
	public class PayabbhiHttpWebRequest : IHttpWebRequest
	{
		readonly HttpWebRequest _request;

		public PayabbhiHttpWebRequest(HttpWebRequest request)
		{
			_request = request;
			_request.KeepAlive = false;
		}

		public string Method
		{
			get { return _request.Method; }
			set { _request.Method = value; }
		}

		public bool KeepAlive
		{
			get { return _request.KeepAlive; }
			set { _request.KeepAlive = value; }
		}

		public string ContentType
		{
			get { return _request.ContentType; }
			set { _request.ContentType = value; }
		}

		public WebHeaderCollection Headers
		{
			get { return _request.Headers; }
			set { _request.Headers = value; }
		}
		public string UserAgent
		{
			get { return _request.UserAgent; }
			set { _request.UserAgent = value; }
		}

		public Stream GetRequestStream()
		{
			return _request.GetRequestStream();
		}

		public IHttpWebResponse GetResponse()
		{
			return new PayabbhiHttpWebResponse((HttpWebResponse)_request.GetResponse());
		}

		public IHttpWebResponse SetErrorResponse(WebResponse response)
		{
			return new PayabbhiHttpWebResponse((HttpWebResponse)response);
		}
	}
}
