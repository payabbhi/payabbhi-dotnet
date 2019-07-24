using System.Collections.Generic;
using Xunit;
using Payabbhi;

namespace UnitTesting.Payabbhi.Tests
{
    public class TestSettlement
    {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string SETTLEMENTID = "settlement_id";
        string settlementUrl = "api/v1/settlements";

        [Fact]
        public void TestGetAllSettlements()
        {
            string filepath = "dummy_settlement_collection.json";
            Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, settlementUrl));
            var result = client.Settlement.All();
            Assert.NotSame(null, result);
        }

        public void TestGetAllSettlementsWithFilters()
        {
            string filepath = "dummy_settlement_collection_filters.json";
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("count", 5);
            options.Add("skip", 2);
            string url = string.Format("{0}?count={1}&skip={2}", settlementUrl, options["count"], options["skip"]);
            Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
            var result = client.Settlement.All(options);
            string expectedJsonString = Helper.GetJsonString(filepath);
            Helper.AssertEntity(result, expectedJsonString);
        }

        public void TestRetrieveSettlement()
        {
            string filepath = "dummy_settlement.json";
            string url = string.Format("{0}/{1}", settlementUrl, SETTLEMENTID);
            Client client = new Client(ACCESSID, SECRETKEY, Helper.GetMockRequestFactory(filepath, url));
            Settlement settlement = client.Settlement.Retrieve(SETTLEMENTID);
            Assert.NotSame(null, settlement);
            string expectedJsonString = Helper.GetJsonString(filepath);
            Helper.AssertEntity(settlement, expectedJsonString);
        }
    }
}