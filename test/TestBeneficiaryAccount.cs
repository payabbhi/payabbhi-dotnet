using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestBeneficiaryAccount {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string BENEFICIARYACCOUNTID = "dummy_beneficiary_account_id";
        readonly string beneficiaryAccountURL = "/api/v1/beneficiaryaccounts";

        [Fact]
        public void TestGetAllBeneficiaryAccounts () {
            string filepath = "dummy_beneficiary_account_collection.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, beneficiaryAccountURL));
            var result = client.BeneficiaryAccount.All ();
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetAllBeneficiaryAccountsWithFilters () {
            string filepath = "dummy_beneficiary_account_collection.json";
            Dictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("count", 2);
            string url = string.Format ("{0}?count={1}", beneficiaryAccountURL, options["count"]);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            var result = client.BeneficiaryAccount.All (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (result, expectedJsonString);
        }

        [Fact]
        public void TestGetBeneficiaryAccountById () {
            string filepath = "dummy_beneficiary_account.json";
            string url = string.Format ("{0}/{1}", beneficiaryAccountURL, BENEFICIARYACCOUNTID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            BeneficiaryAccount beneficiaryAccount = client.BeneficiaryAccount.Retrieve (BENEFICIARYACCOUNTID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (beneficiaryAccount, expectedJsonString);
        }

        [Fact]

        public void TestCreateBeneficiaryAccount () {
            string filepath = "dummy_beneficiary_account.json";
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, beneficiaryAccountURL));
            IDictionary<string, object> options = new Dictionary<string, object> ();
            options.Add ("name", "Bruce Wayne");
            options.Add ("beneficiary_name", "bene_test");
            options.Add ("ifsc", "IFSC0001890");
            options.Add ("bank_account_number", "50100000219");
            options.Add ("account_type", "Savings");
            options.Add ("email", "test@example,com");
            options.Add ("contact_no", "9876543210");
            BeneficiaryAccount beneficiaryAccount = client.BeneficiaryAccount.Create (options);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (beneficiaryAccount, expectedJsonString);
        }
    }
}