using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class PaymentLink : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("email")]
        public string Email { get; set; }

        [JsonProperty ("contact_no")]
        public string ContactNo { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }

        [JsonProperty ("due_date")]
        public int DueDate { get; set; }

        [JsonProperty ("notify_by")]
        public string NotifyBy { get; set; }

        [JsonProperty ("customer_notification_by")]
        public string CustomerNotificationBy { get; set; }

        [JsonProperty ("payment_attempt")]
        public int PaymentAttempt { get; set; }

        [JsonProperty ("receipt_no")]
        public string ReceiptNo { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("url")]
        public string Url { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/payment_links";

        public PaymentLink () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieve a Payment link
        /// </summary>
        /// <returns>Payment Link Object</returns>
        /// <param name="id">The id of the payment link to retrieve</param>
        public PaymentLink Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<PaymentLink>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all payment links
        /// </summary>
        /// <returns>List of payment links</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<PaymentLink> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<PaymentLink>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Creates a new payment link
        /// </summary>
        /// <returns>PaymentLink Object</returns>
        /// <param name="options">Addition Options</param>
        public PaymentLink Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<PaymentLink>.ConvertFromJson (response);
        }

        /// <summary>
        /// Cancel a Payment link
        /// </summary>
        /// <returns>PaymentLink Object</returns>
        /// <param name="id">The id of the payment link to cancel</param>
        public PaymentLink Cancel () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/cancel", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Post, null);
            return Converter<PaymentLink>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all Payments for a Payment link
        /// </summary>
        /// <returns>List of payments</returns>
        /// <param name="id">The id of the payment link to list all payments</param>
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