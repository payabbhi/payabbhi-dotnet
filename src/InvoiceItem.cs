using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class InvoiceItem : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }

        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty ("unit")]
        public string Unit { get; set; }

        [JsonProperty ("quantity")]
        public int Quantity { get; set; }

        [JsonProperty ("discount")]
        public int Discount { get; set; }

        [JsonProperty ("hsn_code")]
        public string Hsn_Code { get; set; }

        [JsonProperty ("sac_code")]
        public string Sac_Code { get; set; }

        [JsonProperty ("tax_rate")]
        public int TaxRate { get; set; }

        [JsonProperty ("cess")]
        public int Cess { get; set; }

        [JsonProperty ("tax_inclusive")]
        public bool TaxInclusive { get; set; }

        [JsonProperty ("tax_amount")]
        public int TaxAmount { get; set; }

        [JsonProperty ("net_amount")]
        public int NetAmount { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty ("deleted_at")]
        public int DeletedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/invoiceitems";

        public InvoiceItem () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves an invoice item object using invoice item id
        /// </summary>
        /// <returns>InvoiceItem Object</returns>
        /// <param name="id">The id of the invoice item to retrieve</param>
        public InvoiceItem Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<InvoiceItem>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all invoice items.
        /// </summary>
        /// <returns>List of invoice items</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<InvoiceItem> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<InvoiceItem>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Create an invoice item
        /// </summary>
        /// <returns>InvoiceItem object</returns>
        /// <param name="options">Additional Options.</param>
        public InvoiceItem Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<InvoiceItem>.ConvertFromJson (response);
        }

        /// <summary>
        /// Delete an invoice item
        /// </summary>
        /// <returns>InvoiceItem object</returns>
        public InvoiceItem Delete () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Delete, null);
            return Converter<InvoiceItem>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all invoices for an invoice item.
        /// </summary>
        /// <returns>List of invoices</returns>
        public PayabbhiList<Invoice> Invoices () {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/invoices", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<PayabbhiList<Invoice>>.ConvertFromJson (response);
        }

    }
}