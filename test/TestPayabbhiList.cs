using Xunit;
using Payabbhi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTesting.Payabbhi.Tests
{
	public class TestPayabbhiList
	{
    const string ACCESSID = "access_id";
		const string SECRETKEY = "secret_key";
		string orderUrl = "api/v1/orders";

		[Fact]
		public void TestGetEnumerator()
		{
      string filepath = "dummy_order_collection.json";
      Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, orderUrl));
			PayabbhiList<Order> orders = client.Order.All();

      string expectedJsonString = Helper.GetJsonString(filepath);
      JToken token = JObject.Parse(expectedJsonString);
      JArray expectedOrders = (JArray)token.SelectToken("data");
      int count = 0;
      foreach (Order order in orders) {
        string item = expectedOrders[count++].ToString(Formatting.None);
        Helper.AssertEntity(order, item);
      }
		}
	}
}
