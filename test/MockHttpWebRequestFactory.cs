using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	public class MockHttpWebRequestFactory : IHttpWebRequestFactory
	{
		public virtual IHttpWebRequest Create(string uri)
		{
			return new MockHttpWebRequest();
		}
	}
}
