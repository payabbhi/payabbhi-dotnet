using System.Collections.Generic;
using NUnit.Framework;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	[TestFixture]
	public class TestEvent
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string EVENTID = "dummy_event_id";
		readonly string eventURL = "/api/v1/events";
		Client client;

		public void Init(string accessID, string secretKey, IHttpWebRequestFactory httpFactory)
		{
			client = new Client(accessID, secretKey, httpFactory);
		}

		[Test]
		public void TestGetAllEvents()
		{
			string filepath = "dummy_event_collection.json";
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, eventURL));
			var result = client.Event.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfEvents(result, expectedJsonString);
		}

		[Test]
		public void TestGetAllEventsWithFilters()
		{
			string filepath = "dummy_event_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", eventURL, options["count"]);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Event.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertListOfEvents(result, expectedJsonString);
		}

		[Test]
		public void TestGetEventById()
		{
			string filepath = "dummy_event.json";
			string url = string.Format("{0}/{1}", eventURL, EVENTID);
			Init(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Event evt = client.Event.Retrieve(EVENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEvent(evt, expectedJsonString);
		}

	}
}
