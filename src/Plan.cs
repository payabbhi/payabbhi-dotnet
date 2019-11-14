using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class Plan : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("product_id")]
        public string ProductId { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("frequency")]
        public int Frequency { get; set; }

        [JsonProperty ("interval")]
        public string Interval { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/plans";

        public Plan () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves a plan object using plan id
        /// </summary>
        /// <returns>Plan Object</returns>
        /// <param name="id">The id of the plan to retrieve</param>
        public Plan Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<Plan>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all plans.
        /// </summary>
        /// <returns>List of plans</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Plan> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Plan>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Create a plan
        /// </summary>
        /// <returns>Plan object</returns>
        /// <param name="options">Additional Options.</param>
        public Plan Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<Plan>.ConvertFromJson (response);
        }

    }
}