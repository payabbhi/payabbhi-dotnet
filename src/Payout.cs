using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class Payout : PayabbhiEntity {

        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("beneficiary_name")]
        public string BeneficiaryName { get; set; }

        [JsonProperty ("merchant_reference_id")]
        public string MerchantReferenceId { get; set; }

        [JsonProperty ("beneficiary_id")]
        public string BeneficiaryId { get; set; }

        [JsonProperty ("remittance_account_no")]
        public string RemittanceAccountNo { get; set; }

        [JsonProperty ("beneficiary_account_no")]
        public string BeneficiaryAccountNo { get; set; }

        [JsonProperty ("beneficiary_ifsc")]
        public string BeneficiaryIfsc { get; set; }

        [JsonProperty ("method")]
        public string Method { get; set; }

        [JsonProperty ("purpose")]
        public string Purpose { get; set; }

        [JsonProperty ("narration")]
        public string Narration { get; set; }

        [JsonProperty ("instrument")]
        public string Instrument { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("utr")]
        public string Utr { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/payouts";

        public Payout () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves a payout object using payout id
        /// </summary>
        /// <returns>Payout Object</returns>
        /// <param name="id">The id of the payout to retrieve</param>
        public Payout Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<Payout>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all payouts.
        /// </summary>
        /// <returns>List of payouts</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Payout> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Payout>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Create a payout
        /// </summary>
        /// <returns>Payout object</returns>
        /// <param name="options">Additional Options.</param>
        public Payout Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<Payout>.ConvertFromJson (response);
        }

    }
}
