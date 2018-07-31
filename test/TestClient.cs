using NUnit.Framework;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestClient
	{
		[Test]
		public void TestGetBaseUrl()
		{
			string baseUrl = Client.BaseUrl;
			string expectedBaseUrl = "https://payabbhi.com";
			Assert.AreSame(expectedBaseUrl, baseUrl);
		}

		[Test]
		public void TestSetBaseUrl()
		{
			string setBaseUrl = "https://random.com";
			Client.BaseUrl = setBaseUrl;
			Assert.AreSame(Client.BaseUrl, setBaseUrl);
		}

		[Test]
		public void TestSetAppInfo()
		{
			string appName = "TestApp";
			string appVersion = "1.00";
			string appUrl = "http://www.testapp.com";
			Client.setAppInfo(appName, appVersion, appUrl);
			Assert.AreSame(Client.AppInfo["name"], appName);
			Assert.AreSame(Client.AppInfo["version"], appVersion);
			Assert.AreSame(Client.AppInfo["url"], appUrl);
		}
	}
}
