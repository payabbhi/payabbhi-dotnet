using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class Invoice : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("billing_method")]
        public string BillingMethod { get; set; }

        [JsonProperty ("customer_id")]
        public string CustomerId { get; set; }

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

        [JsonProperty ("invoice_no")]
        public string InvoiceNo { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("subscription_id")]
        public string SubscriptionId { get; set; }

        [JsonProperty ("url")]
        public string Url { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("place_of_supply")]
        public string PlaceOfSupply { set; get; }

        [JsonProperty ("gross_amount")]
        public int GrossAmount { set; get; }

        [JsonProperty ("tax_amount")]
        public int TaxAmount { set; get; }

        [JsonProperty ("customer_notes")]
        public string CustomerNotes { get; set; }

        [JsonProperty ("terms_conditions")]
        public string TermsConditions { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty ("issued_at")]
        public int IssuedAt { get; set; }

        [JsonProperty ("voided_at")]
        public int VoidedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/invoices";

        public Invoice () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves an invoice object using invoice id
        /// </summary>
        /// <returns>Invoice Object</returns>
        /// <param name="id">The id of the invoice to retrieve</param>
        public Invoice Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<Invoice>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all invoices.
        /// </summary>
        /// <returns>List of invoices</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Invoice> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Invoice>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Create an invoice
        /// </summary>
        /// <returns>Invoice object</returns>
        /// <param name="options">Additional Options.</param>
        public Invoice Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<Invoice>.ConvertFromJson (response);
        }

        /// <summary>
        /// Void an invoice
        /// </summary>
        /// <returns>Invoice object</returns>
        public Invoice Void () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/void", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Post, null);
            return Converter<Invoice>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all line items for an invoice.
        /// </summary>
        /// <returns>List of line items</returns>
        public PayabbhiList<InvoiceItem> LineItems () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/line_items", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<PayabbhiList<InvoiceItem>>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all payments for an invoice.
        /// </summary>
        /// <returns>List of payments</returns>
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