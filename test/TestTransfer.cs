using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestTransfer {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string TRANSFERID = "dummy_transfer_id";
        readonly string transferURL = "/api/v1/transfers";

        [Fact]
        public void TestGetAllTransfers () {
            string filepath = "dummy_transfer_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, transferURL));
            var result = client.Transfer.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllTransfersWithFilters () {
            string filepath = "dummy_transfer_collection.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 2);
            string url = string.Format ("{0}?count={1}", transferURL, options["count"]);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.Transfer.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetTransferById () {
            string filepath = "dummy_transfer.json";
            string url = string.Format ("{0}/{1}", transferURL, TRANSFERID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            Transfer transfer = client.Transfer.Retrieve (TRANSFERID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (transfer, expectedJsonString);
        }

        [Fact]
        public void TestCreateTransfer () {
            string filepath = "dummy_direct_transfer.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, transferURL));
            Dictionary<string, object> transferItem = new Dictionary<string, object> () { { "amount", 100 }, { "currency", "INR" }, { "description", "Tranfer 1" }, { "beneficiary_id", "bene_Za30i2k3p6blq3i1" }
            };
            Dictionary<string, object>[] transferArr = { transferItem };
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("transfers", transferArr);
            PayabbhiList<Transfer> transfer = client.Transfer.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (transfer, expectedJsonString);
        }
    }
}