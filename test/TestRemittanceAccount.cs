using System.Collections.Generic;
using Payabbhi;
using Payabbhi.Error;
using Xunit;

namespace UnitTesting.Payabbhi.Tests {
    public class TestRemittanceAccount {
        const string ACCESSID = "access_id";
        const string SECRETKEY = "secret_key";
        const string REMITTANCEACCOUNTID = "dummy_remittance_account_id";
        readonly string remittanceAccountURL = "/api/v1/remittance_accounts";

        [Fact]
        public void TestGetRemittanceAccountById () {
            string filepath = "dummy_remittance_account.json";
            string url = string.Format ("{0}/{1}", remittanceAccountURL, REMITTANCEACCOUNTID);
            Client client = new Client (ACCESSID, SECRETKEY, Helper.GetMockRequestFactory (filepath, url));
            RemittanceAccount remittanceAccount = client.RemittanceAccount.Retrieve (REMITTANCEACCOUNTID);
            string expectedJsonString = Helper.GetJsonString (filepath);
            Helper.AssertEntity (remittanceAccount, expectedJsonString);
        }

    }
}
