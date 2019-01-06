using System.Collections.Generic;
using Xunit;
using Payabbhi;
using Payabbhi.Error;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestEvent
	{
		const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		const string EVENTID = "dummy_event_id";
		readonly string eventURL = "/api/v1/events";

		[Fact]
		public void TestGetAllEvents()
		{
			string filepath = "dummy_event_collection.json";
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, eventURL));
			var result = client.Event.All();
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetAllEventsWithFilters()
		{
			string filepath = "dummy_event_collection.json";
			Dictionary<string, object> options = new Dictionary<string, object>();
			options.Add("count", 2);
			string url = string.Format("{0}?count={1}", eventURL, options["count"]);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			var result = client.Event.All(options);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(result, expectedJsonString);
		}

		[Fact]
		public void TestGetEventById()
		{
			string filepath = "dummy_event.json";
			string url = string.Format("{0}/{1}", eventURL, EVENTID);
			Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
			Event evt = client.Event.Retrieve(EVENTID);
			string expectedJsonString = Helper.GetJsonString(filepath);
			Helper.AssertEntity(evt, expectedJsonString);
		}

	}
}
