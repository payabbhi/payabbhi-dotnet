using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;

namespace Payabbhi
{
	[ExcludeFromCodeCoverage]
	public class PayabbhiHttpWebResponse : IHttpWebResponse
	{
		readonly WebResponse _response;
		HttpStatusCode httpStatusCode;

		public HttpStatusCode StatusCode
		{
			get
			{
				return httpStatusCode;
			}
			set
			{
				httpStatusCode = value;
			}
		}

		public PayabbhiHttpWebResponse(HttpWebResponse response)
		{
			_response = response;
			httpStatusCode = response.StatusCode;
		}

		public Stream GetResponseStream()
		{
			return _response.GetResponseStream();
		}
	}
}
