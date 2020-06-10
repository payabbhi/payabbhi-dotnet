using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class RemittanceAccount : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("balance_amount")]
        public string BalanceAmount { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("account_type")]
        public string AccountType { get; set; }

        [JsonProperty ("ifsc")]
        public string Ifsc { get; set; }

        [JsonProperty ("beneficiary_name")]
        public string BeneficiaryName { get; set; }

        [JsonProperty ("low_balance_alert")]
        public bool LowBalanceAlert { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/remittance_accounts";

        public RemittanceAccount () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieve a Remittance Account
        /// </summary>
        /// <returns>remittance account object</returns>
        /// <param name="id">The id of the remittance account to retrieve</param>
        public RemittanceAccount Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<RemittanceAccount>.ConvertFromJson (response);
        }


    }
}
