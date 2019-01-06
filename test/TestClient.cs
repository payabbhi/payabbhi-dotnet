using Xunit;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestClient
	{
		[Fact]
		public void TestGetBaseUrl()
		{
			string baseUrl = Client.BaseUrl;
			string expectedBaseUrl = "https://payabbhi.com";
			Assert.Same(expectedBaseUrl, baseUrl);
		}

		[Fact]
		public void TestSetBaseUrl()
		{
			string initialBaseUrl = Client.BaseUrl;
			string setBaseUrl = "https://random.com";
			Client.BaseUrl = setBaseUrl;
			Assert.Same(Client.BaseUrl, setBaseUrl);
			Client.BaseUrl = initialBaseUrl;
		}

		[Fact]
		public void TestSetAppInfo()
		{
			string appName = "TestApp";
			string appVersion = "1.00";
			string appUrl = "http://www.testapp.com";
			Client.setAppInfo(appName, appVersion, appUrl);
			Assert.Same(Client.AppInfo["name"], appName);
			Assert.Same(Client.AppInfo["version"], appVersion);
			Assert.Same(Client.AppInfo["url"], appUrl);
		}
	}
}
