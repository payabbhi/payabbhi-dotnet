using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class HttpClient
	{
		/// <summary>
		/// Make request to Payabbhi API service.
		/// </summary>
		/// <returns>Response from the API service</returns>
		/// <param name="relativeUrl">Relative URL.</param>
		/// <param name="method">Method.</param>
		/// <param name="options">Options.</param>
		public PayabbhiResponse Request(string relativeUrl, HttpMethod method, IDictionary<string, object> options)
		{
			string queryString = string.Empty;
			string postData = string.Empty;
			if (method == HttpMethod.Get && options != null)
			{
				queryString = MakeQueryString(options);
				relativeUrl = relativeUrl + "?" + queryString;
			}
			else if (method == HttpMethod.Post)
			{
				postData = JsonConvert.SerializeObject(options);
			}
			string responseText = MakeRequest(relativeUrl, method, postData);
			PayabbhiResponse response = new PayabbhiResponse();
			response.ResponseJson = responseText;
			return response;
		}

		string MakeQueryString(IDictionary<string, object> options)
		{
			var list = new List<string>();
			foreach (var option in options)
			{
				string param = string.Format("{0}={1}", option.Key, option.Value);
				list.Add(param);
			}
			return string.Join("&", list);
		}

		string MakeRequest(string relativeUrl, HttpMethod method, string data)
		{
			IHttpWebRequest request = createRequest(relativeUrl, method);

			if (method == HttpMethod.Post)
			{
				var dataBytes = Encoding.UTF8.GetBytes(data);

				try
				{
					using (var writeStream = request.GetRequestStream())
					{
						writeStream.Write(dataBytes, 0, dataBytes.Length);
					}
				}
				catch (WebException)
				{
					throw new Error.ApiConnectionError(Constants.Messages.ApiConnectionError, null, null);
				}
			}
			return createResponse(request);
		}

		IHttpWebRequest createRequest(string relativeUrl, HttpMethod method)
		{
			IHttpWebRequest request = Client.HttpWebRequestClientFactory.Create(Client.BaseUrl + relativeUrl);
			request.Method = method.ToString();
			request.ContentType = "application/json";
			request.KeepAlive = false;
			string authString = string.Format("{0}:{1}", Client.AccessID, Client.SecretKey);
			request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));
			string useragent = getUA();
			request.UserAgent = useragent;
			string clientUserAgent = getClientUserAgentString();
			request.Headers["X-Payabbhi-Client-User-Agent"] = clientUserAgent;
			return request;
		}

		string getUA()
		{
			string useragent = "Payabbhi/v1 .NetBindings/" + Client.Version;
			IDictionary<string, string> appInfo = Client.AppInfo;
			if (appInfo != null)
			{
				useragent += " " + formatAppInfo(appInfo);
			}
			return useragent;
		}

		string formatAppInfo(IDictionary<string, string> appInfo)
		{
			string appInfoStr = appInfo["name"];
			if (appInfo["version"] != null)
			{
				appInfoStr += '/' + appInfo["version"];
			}
			if (appInfo["url"] != null)
			{
				appInfoStr += " (" + appInfo["url"] + ")";
			}
			return appInfoStr;
		}

		string getClientUserAgentString()
		{
			var langVersion = "4.0";
#if NET45
            langVersion = "4.5";
#endif
			var values = new Dictionary<string, object>
		  {
			  { "bindings_version", Client.Version },
			  { "lang", ".net" },
			  { "publisher", "payabbhi" },
			  { "lang_version", WebUtility.HtmlEncode(langVersion) },
			  { "uname", WebUtility.HtmlEncode(getSystemInformation()) }
		  };
			IDictionary<string, string> appInfo = Client.AppInfo;
			if (appInfo != null)
			{
				values.Add("application", appInfo);
			}
			return JsonConvert.SerializeObject(values, Formatting.None);
		}

		string getSystemInformation()
		{
			OSHelper h = new OSHelper();
			CurrentOS os = new CurrentOS(Path.DirectorySeparatorChar, Environment.OSVersion.VersionString, Environment.Is64BitOperatingSystem, h);
			return string.Format("{0} {1}", Environment.MachineName, os.Name);
		}

		string createResponse(IHttpWebRequest request)
		{
			var responseValue = string.Empty;
			IHttpWebResponse response = null;
			try
			{
				response = request.GetResponse();
				if (response == null)
				{
					throw new Error.ApiConnectionError(Constants.Messages.ApiConnectionError, null, null);
				}
				responseValue = ParseResponse(response);
			}
			catch (WebException e)
			{
				response = request.SetErrorResponse(e.Response);
				responseValue = ParseResponse(response);
				HandleErrors(response.StatusCode, responseValue);
			}

			return responseValue;
		}

		string ParseResponse(IHttpWebResponse response)
		{
			string responseValue = string.Empty;
			using (var responseStream = response.GetResponseStream())
			{
				if (responseStream != null)
					using (var reader = new StreamReader(responseStream))
					{
						responseValue = reader.ReadToEnd();
					}
			}

			return responseValue;
		}

		void HandleErrors(HttpStatusCode httpStatusCode, string response)
		{
			PayabbhiResponse payabbhiResponse = new PayabbhiResponse();
			payabbhiResponse.ResponseJson = response;
			dynamic data = null;
			string field = string.Empty;
			string message = string.Empty;
			try
			{
				data = JsonConvert.DeserializeObject(response);
				field = data["error"]["field"];
				message = data["error"]["message"];
			}
			catch (Exception)
			{
				throw Error.ErrorFactory.Create(Constants.Messages.ApiError, null, payabbhiResponse, HttpStatusCode.InternalServerError);
			}

			throw Error.ErrorFactory.Create(message, field, payabbhiResponse, httpStatusCode);
		}
	}
}
