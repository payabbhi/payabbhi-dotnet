using System.Collections.Generic;
using System.Reflection;

namespace Payabbhi
{
	public class Client
	{
		protected static string baseUrl = "https://payabbhi.com";
		protected static string accessID;
		protected static string secretKey;
		protected static string version = new AssemblyName(typeof(Payment).Assembly.FullName).Version.ToString(3);
		protected static IHttpWebRequestFactory httpWebRequestFactory;
		Payment payment;
		Order order;
		Refund refund;
		Utility utility;
		static IDictionary<string, string> appInfo;

		public Client(string accessID, string secretKey, IHttpWebRequestFactory _httpWebRequestFactory = null)
		{
			Client.AccessID = accessID;
			Client.SecretKey = secretKey;
			if (_httpWebRequestFactory == null)
			{
				Client.httpWebRequestFactory = new PayabbhiHttpWebRequestFactory();
			}
			else
			{
				Client.httpWebRequestFactory = _httpWebRequestFactory;
			}
		}

		/// <summary>
		/// Gets or sets the base URL.
		/// </summary>
		/// <value>The base URL.</value>
		public static string BaseUrl
		{
			get
			{
				return baseUrl;
			}
			set
			{
				baseUrl = value;
			}
		}

		public static IHttpWebRequestFactory HttpWebRequestClientFactory
		{
			get
			{
				return httpWebRequestFactory;
			}
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <value>The version.</value>
		public static string Version
		{
			get
			{
				return version;
			}
		}

		/// <summary>
		/// Gets the accessId.
		/// </summary>
		/// <value>AccessID.</value>
		public static string AccessID
		{
			get
			{
				return accessID;
			}
			private set
			{
				accessID = value;
			}
		}

		/// <summary>
		/// Gets the secret key.
		/// </summary>
		/// <value>The secret key.</value>
		public static string SecretKey
		{
			get
			{
				return secretKey;
			}
			private set
			{
				secretKey = value;
			}
		}

		public Payment Payment
		{
			get
			{
				if (payment == null)
				{
					payment = new Payment();
				}
				return payment;
			}
		}

		public Order Order
		{
			get
			{
				if (order == null)
				{
					order = new Order();
				}
				return order;
			}
		}

		public Refund Refund
		{
			get
			{
				if (refund == null)
				{
					refund = new Refund();
				}
				return refund;
			}
		}

		public Utility Utility
		{
			get
			{
				if (utility == null)
				{
					utility = new Utility();
				}
				return utility;
			}
		}

		/// <summary>
		/// Gets the app info.
		/// </summary>
		/// <value>The app info.</value>
		public static IDictionary<string, string> AppInfo
		{
			get
			{
				return appInfo;
			}
		}

		/// <summary>
		/// Sets the app info.
		/// </summary>
		/// <param name="appName">App name.</param>
		/// <param name="appVersion">App version.</param>
		/// <param name="appUrl">App URL.</param>
		public static void setAppInfo(string appName, string appVersion = null, string appUrl = null)
		{
			appInfo = new Dictionary<string, string>();
			appInfo.Add("name", appName);
			appInfo.Add("version", appVersion);
			appInfo.Add("url", appUrl);
		}
	}
}
