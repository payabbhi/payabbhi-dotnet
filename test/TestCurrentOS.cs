using NSubstitute;
using NUnit.Framework;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestCurrentOS
	{
		[Test]
		public void TestMacOS64bit()
		{
			var osHelper = Substitute.For<OSHelper>();
			osHelper.ReadProcessOutput("uname", null).Returns("Darwin");
			osHelper.ReadProcessOutput("sw_vers", "-productVersion").Returns("10.12.6");
			CurrentOS os = new CurrentOS('/', "Unix 16.7.0.0", true, osHelper);
			Assert.AreEqual(os.Name, "Darwin MacOS X 10.12.6 64bit");
		}

		[Test]
		public void TestMacOS32bit()
		{
			var osHelper = Substitute.For<OSHelper>();
			osHelper.ReadProcessOutput("uname", null).Returns("Darwin");
			osHelper.ReadProcessOutput("sw_vers", "-productVersion").Returns("10.12.6");
			CurrentOS os = new CurrentOS('/', "Unix 16.7.0.0", false, osHelper);
			Assert.AreEqual(os.Name, "Darwin MacOS X 10.12.6 32bit");
		}

		[Test]
		public void TestWindowsOS64bit()
		{
			var osHelper = Substitute.For<OSHelper>();
			CurrentOS os = new CurrentOS('\\', "Microsoft Windows NT 10.0.14393.0", true, osHelper);
			Assert.AreEqual(os.Name, "Windows 10 64bit 10.0.14393.0");
		}

		[Test]
		public void TestWindowsOS32bit()
		{
			var osHelper = Substitute.For<OSHelper>();
			CurrentOS os = new CurrentOS('\\', "Microsoft Windows NT 10.0.14393.0", false, osHelper);
			Assert.AreEqual(os.Name, "Windows 10 32bit 10.0.14393.0");
		}

		[Test]
		public void TestUnixOS()
		{
			var osHelper = Substitute.For<OSHelper>();
			osHelper.ReadProcessOutput("uname", null).Returns("UnixName");
			CurrentOS os = new CurrentOS('/', "Unix 16.7.0.0", true, osHelper);
			Assert.AreEqual(os.Name, "Unix");
		}

		[Test]
		public void TestEmptyUnixName()
		{
			var osHelper = Substitute.For<OSHelper>();
			osHelper.ReadProcessOutput("uname", null).Returns("");
			CurrentOS os = new CurrentOS('/', "Unix 16.7.0.0", true, osHelper);
			Assert.AreEqual(os.Name, "Unknown");
		}

		[Test]
		public void TestReadProcessOutput()
		{
			OSHelper h = new OSHelper();
			h.ReadProcessOutput("sw_vers", "-productVersion");
		}
	}
}
