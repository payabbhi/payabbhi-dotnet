using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestPayout {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string PAYOUTID = "dummy_payout_id";
        readonly string payoutURL = "/api/v1/payouts";

        [Fact]
        public void TestGetAllPayouts () {
            string filepath = "dummy_payout_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, payoutURL));
            var result = client.Payout.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllPayoutsWithFilters () {
            string filepath = "dummy_payout_collection.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 2);
            string url = string.Format ("{0}?count={1}", payoutURL, options["count"]);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.Payout.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetPayoutById () {
            string filepath = "dummy_payout.json";
            string url = string.Format ("{0}/{1}", payoutURL, PAYOUTID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Payout payout = client.Payout.Retrieve (PAYOUTID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (payout, expectedJsonString);
        }

        [Fact]
        public void TestCreatePayout () {
            string filepath = "dummy_payout.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, payoutURL));
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("amount", 1000);
            options.Add ("currency", "INR");
            options.Add ("merchant_reference_id", "ref_00001");
            options.Add ("remittance_account_no", "1234567890");
            options.Add ("beneficiary_account_no", "01234567890");
            options.Add ("beneficiary_ifsc", "ABCD1234567");
            options.Add ("beneficiary_name", "BenTest");
            options.Add ("method", "bank_transfer");
            options.Add ("purpose", "cashback");
            options.Add ("narration", "info");
            options.Add ("instrument", "NEFT");
            Payout payout = client.Payout.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (payout, expectedJsonString);
        }
    }
}
