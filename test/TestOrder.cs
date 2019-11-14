using System.Collections.Generic;
using System.IO;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestOrder {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string ORDERID = "dummy_order_id";
        string orderUrl = "api/v1/orders";

        [Fact]
        public void TestGetAllOrders () {
            string filepath = "dummy_order_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, orderUrl));
            var result = client.Order.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllOrdersWithFilters () {
            string filepath = "dummy_order_collection_filters.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 3);
            options.Add ("skip", 2);
            string url = string.Format ("{0}?count={1}&skip={2}", orderUrl, options["count"], options["skip"]);

            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.Order.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestRetrieveOrder () {
            string filepath = "dummy_order.json";
            string url = string.Format ("{0}/{1}", orderUrl, ORDERID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Order order = client.Order.Retrieve (ORDERID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (order, expectedJsonString);
        }

        [Fact]
        public void TestCreateOrder () {
            string filepath = "dummy_order.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, orderUrl));
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("merchant_order_id", "mer_4gp");
            options.Add ("amount", 1000);
            options.Add ("currency", "INR");
            Order order = client.Order.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (order, expectedJsonString);
        }

        [Fact]
        public void TestGetPaymentsByOrder () {
            string filepath = "dummy_order.json";
            string url = string.Format ("{0}/{1}", orderUrl, ORDERID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Order order = client.Order.Retrieve (ORDERID);

            string filepath2 = "dummy_order_payments.json";
            string url2 = string.Format ("{0}/{1}/payments", orderUrl, ORDERID);
            client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath2, url2));
            var payments = order.Payments ();
            string expectedJsonString = Helper.GetJsonString (filepath2);
            Helper.AssertEntity (payments, expectedJsonString);
        }

        [Fact]
        public void TestEmptyOrderRetrievesPayment () {
            string filepath = "dummy_order_payments.json";
            string url2 = string.Format ("{0}/{1}/payments", orderUrl, ORDERID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url2));
            var ex = Assert.Throws<InvalidRequestError> (() => client.Order.Payments ());
            Assert.Equal (ex.Message, "message: Object Id not set\n");
            Assert.Equal (ex.Description, Constants.Messages.InvalidCallError);
        }
    }
}