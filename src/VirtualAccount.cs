using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class VirtualAccount : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("paid_amount")]
        public int PaidAmount { get; set; }

        [JsonProperty ("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("contact_no")]
        public string ContactNo { get; set; }

        [JsonProperty ("order_id")]
        public string OrderId { get; set; }

        [JsonProperty ("invoice_id")]
        public string InvoiceId { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }

        [JsonProperty ("collection_methods")]
        public List<Object> CollectionMethods { get; set; }

        [JsonProperty ("notification_method")]
        public string NotificationMethod { get; set; }

        [JsonProperty ("customer_notification_by")]
        public string CustomerNotificationBy { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/virtual_accounts";

        public VirtualAccount () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieve a Virtual Account
        /// </summary>
        /// <returns>virtual account object</returns>
        /// <param name="id">The id of the virtual account to retrieve</param>
        public VirtualAccount Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<VirtualAccount>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all virtual account
        /// </summary>
        /// <returns>List of virtual account</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<VirtualAccount> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<VirtualAccount>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Creates a new virtual account
        /// </summary>
        /// <returns>virtual account object</returns>
        /// <param name="options">Addition Options</param>
        public VirtualAccount Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<VirtualAccount>.ConvertFromJson (response);
        }

        /// <summary>
        /// Delete a virtual account
        /// </summary>
        /// <returns>virtual account object</returns>
        /// <param name="id">The id of the virtual account to delete</param>
        public VirtualAccount Close () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Patch, null);
            return Converter<VirtualAccount>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all Payments for a virtual account
        /// </summary>
        /// <returns>List of payments</returns>
        /// <param name="id">The id of the virtual account to list all payments</param>
        public PayabbhiList<Payment> Payments () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/payments", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<PayabbhiList<Payment>>.ConvertFromJson (response);
        }
    }
}