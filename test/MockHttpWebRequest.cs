using System.IO;
using System.Net;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	public class MockHttpWebRequest : IHttpWebRequest
	{
		HttpWebRequest request;
		MemoryStream requestStream = new MemoryStream();

		public HttpWebRequest Request
		{
			get { return request; }
			set { request = value; }
		}

		public string Method
		{
			get { return request.Method; }
			set { request.Method = value; }
		}

    public bool KeepAlive
   	{
      get { return request.KeepAlive; }
     	set { request.KeepAlive = value; }
    }

		public string ContentType
		{
			get { return request.ContentType; }
			set { request.ContentType = value; }
		}

		public WebHeaderCollection Headers
		{
			get { return request.Headers; }
			set { request.Headers = value; }
		}
		public string UserAgent
		{
			get { return request.UserAgent; }
			set { request.UserAgent = value; }
		}

		public virtual Stream GetRequestStream()
		{
			return requestStream;
		}

		public virtual IHttpWebResponse GetResponse()
		{
			MockHttpWebResponse mockResponse = new MockHttpWebResponse();
			mockResponse.Response = (HttpWebResponse)request.GetResponse();
			return mockResponse;
		}

		public virtual IHttpWebResponse SetErrorResponse(WebResponse response)
		{
			MockHttpWebResponse mockResponse = new MockHttpWebResponse();
			return mockResponse;
		}
	}
}
