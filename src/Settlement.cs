using System.Collections.Generic;
using Newtonsoft.Json;

namespace Payabbhi
{
    public class Settlement : PayabbhiEntity
    {
        [JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("fees")]
        public int Fees { get; set; }

        [JsonProperty("gst")]
        public int Gst { get; set; }

        [JsonProperty("utr")]
        public string Utr { get; set; }

        [JsonProperty("settled_at")]
        public int SettledAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/settlements";

        public Settlement()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Retrieves a settlement object using settlement id.
        /// </summary>
        /// <returns>Settlement Object</returns>
        /// <param name="id">The id of the settlement to retrieve</param>
        public Settlement Retrieve(string id)
        {
            string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
            return Converter<Settlement>.ConvertFromJson(response);
        }

        /// <summary>
        /// List all settlements
        /// </summary>
        /// <returns>List of settlements</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Settlement> All(IDictionary<string, object> options = null)
        {
            var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Settlement>>.ConvertFromJson(response);
        }
    }
}