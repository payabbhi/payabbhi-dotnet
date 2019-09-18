using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class Subscription : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("plan_id")]
        public string PlanId { get; set; }

        [JsonProperty ("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty ("billing_method")]
        public string BillingMethod { get; set; }

        [JsonProperty ("quantity")]
        public int Quantity { get; set; }

        [JsonProperty ("customer_notification_by")]
        public string CustomerNotificationBy { get; set; }

        [JsonProperty ("billing_cycle_count")]
        public int BillingCycleCount { get; set; }

        [JsonProperty ("paid_count")]
        public int PaidCount { get; set; }

        [JsonProperty ("cancel_at_period_end")]
        public bool CancelAtPeriodEnd { get; set; }

        [JsonProperty ("due_at")]
        public int DueAt { get; set; }

        [JsonProperty ("trial_end_at")]
        public int TrialEndAt { get; set; }

        [JsonProperty ("trial_duration")]
        public int TrialDuration { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("current_start_at")]
        public int CurrentStartAt { get; set; }

        [JsonProperty ("current_end_at")]
        public int CurrentEndAt { get; set; }

        [JsonProperty ("ended_at")]
        public int EndedAt { get; set; }

        [JsonProperty ("cancelled_at")]
        public int CancelledAt { get; set; }

        [JsonProperty ("notes")]
        public object Notes { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty ("voided_at")]
        public int VoidedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/subscriptions";

        public Subscription () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves a subscription object using subscription id
        /// </summary>
        /// <returns>Subscription Object</returns>
        /// <param name="id">The id of the subscription to retrieve</param>
        public Subscription Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<Subscription>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all subscriptions.
        /// </summary>
        /// <returns>List of subscriptions</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Subscription> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Subscription>>.ConvertFromJson (response);
        }

        /// <summary>
        /// Create a subscription
        /// </summary>
        /// <returns>Subscription object</returns>
        /// <param name="options">Additional Options.</param>
        public Subscription Create (IDictionary<string, object> options) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Post, options);
            return Converter<Subscription>.ConvertFromJson (response);
        }

        /// <summary>
        /// Cancel a subscription
        /// </summary>
        /// <returns>Subscription object</returns>
        public Subscription Cancel (IDictionary<string, object> options = null) {
            string id = this.Id;
            if (String.IsNullOrEmpty (id)) {
                throw new Error.InvalidRequestError (Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
            }
            string requestUrl = string.Format ("{0}/{1}/cancel", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Post, null);
            return Converter<Subscription>.ConvertFromJson (response);
        }

    }
}