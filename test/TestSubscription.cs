using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestSubscription {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string SUBSCRIPTIONID = "dummy_subscription_id";
        readonly string subscriptionURL = "/api/v1/subscriptions";

        [Fact]
        public void TestGetAllSubscriptions () {
            string filepath = "dummy_subscription_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, subscriptionURL));
            var result = client.Subscription.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllSubscriptionWithFilters () {
            string filepath = "dummy_subscription_collection.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 2);
            string url = string.Format ("{0}?count={1}", subscriptionURL, options["count"]);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.Subscription.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetSubscriptionById () {
            string filepath = "dummy_subscription.json";
            string url = string.Format ("{0}/{1}", subscriptionURL, SUBSCRIPTIONID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Subscription subscription = client.Subscription.Retrieve (SUBSCRIPTIONID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (subscription, expectedJsonString);
        }

        [Fact]
        public void TestCreateSubscription () {
            string filepath = "dummy_subscription.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, subscriptionURL));
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("plan_id", "plan_tuOWN0Sc0uMB4s8E");
            options.Add ("customer_id", "cust_2WmsQoSRZMWWkcZg");
            options.Add ("billing_cycle_count", 5);
            Subscription subscription = client.Subscription.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (subscription, expectedJsonString);
        }

        [Fact]
        public void TestCancelSubscription () {
            string filepath = "dummy_subscription.json";
            string url = string.Format ("{0}/{1}", subscriptionURL, SUBSCRIPTIONID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Subscription subscription = client.Subscription.Retrieve (SUBSCRIPTIONID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (subscription, expectedJsonString);

            string filepath2 = "dummy_subscription_cancel.json";
            string cancel_url = string.Format ("{0}/cancel", subscriptionURL);
            client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath2, cancel_url));
            subscription = subscription.Cancel ();
            expectedJsonString = Helper.GetJsonString (filepath2);
            Helper.AssertEntity (subscription, expectedJsonString);
        }

        [Fact]
        public void TestEmptyCancelSubscription () {
            string filepath = "dummy_subscription_cancel.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, subscriptionURL));
            var ex = Assert.Throws<InvalidRequestError> (() => client.Subscription.Cancel ());
            Assert.Equal (ex.Message, "message: Object Id not set\n");
            Assert.Equal (ex.Description, Constants.Messages.InvalidCallError);
        }
    }
}