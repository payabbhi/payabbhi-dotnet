using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestPlan {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string PLANID = "dummy_plan_id";
        readonly string planURL = "/api/v1/plans";

        [Fact]
        public void TestGetAllPlans () {
            string filepath = "dummy_plan_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, planURL));
            var result = client.Plan.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllPlansWithFilters () {
            string filepath = "dummy_plan_collection.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 2);
            string url = string.Format ("{0}?count={1}", planURL, options["count"]);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.Plan.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetPlanById () {
            string filepath = "dummy_plan.json";
            string url = string.Format ("{0}/{1}", planURL, PLANID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Plan plan = client.Plan.Retrieve (PLANID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (plan, expectedJsonString);
        }

        [Fact]
        public void TestCreatePlan () {
            string filepath = "dummy_plan.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, planURL));
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("product_id", "prod_wJ6DyX5Bgg2LqAqt");
            options.Add ("amount", 100);
            options.Add ("currency", "INR");
            options.Add ("frequency", 2);
            options.Add ("interval", "month(s)");
            Plan plan = client.Plan.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (plan, expectedJsonString);
        }
    }
}