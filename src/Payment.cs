using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Payment : PayabbhiEntity
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

		[JsonProperty("order_id")]
		public string OrderId { get; set; }

		[JsonProperty("international")]
		public bool International { get; set; }

		[JsonProperty("method")]
		public string Method { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("card")]
		public string Card { get; set; }

		[JsonProperty("bank")]
		public string Bank { get; set; }

		[JsonProperty("wallet")]
		public string Wallet { get; set; }

		[JsonProperty("emi")]
		public EmiInfo Emi { get; set; }

		[JsonProperty("vpa")]
		public string VPA { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("contact")]
		public string Contact { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("fee")]
		public int Fee { get; set; }

		[JsonProperty("service_tax")]
		public int ServiceTax { get; set; }

		[JsonProperty("payout_amount")]
		public int PayoutAmount { get; set; }

		[JsonProperty("payout_type")]
		public string PayoutType { get; set; }

		[JsonProperty("refunded_amount")]
		public int RefundedAmount { get; set; }

		[JsonProperty("refund_status")]
		public string RefundStatus { get; set; }

		[JsonProperty("refunds")]
		public PayabbhiList<Refund> Refunds { get; set; }

		[JsonProperty("error_code")]
		public string ErrorCode { get; set; }

		[JsonProperty("error_description")]
		public string ErrorDescription { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/payments";

		public Payment()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a payment object using payment id
		/// </summary>
		/// <returns>Payment Object</returns>
		/// <param name="id">The id of the payment to retrieve</param>
		public Payment Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Payment>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all payments.
		/// </summary>
		/// <returns>List of payments</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Payment> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Payment>>.ConvertFromJson(response);
		}

		/// <summary>
		/// Captures a payment.
		/// </summary>
		/// <returns>Payment object</returns>
		/// <param name="options">Additional Options</param>
		public Payment Capture(IDictionary<string, object> options = null)
		{
			string id = this.Id;
			if (String.IsNullOrEmpty(id))
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
			}
			string requestUrl = string.Format("{0}/{1}/capture", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Post, options);
			return Converter<Payment>.ConvertFromJson(response);
		}

		/// <summary>
		/// Refunds a payment.
		/// </summary>
		/// <returns>Refund object</returns>
		/// <param name="options">Additional Options</param>
		public Refund Refund(IDictionary<string, object> options = null)
		{
			string id = this.Id;
			if (String.IsNullOrEmpty(id))
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
			}
			string requestUrl = string.Format("{0}/{1}/refunds", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Post, options);
			return Converter<Refund>.ConvertFromJson(response);
		}

		/// <summary>
		/// List refunds for a given payment
		/// </summary>
		/// <returns>List of refunds for a payment</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Refund> GetRefunds(IDictionary<string, object> options = null)
		{
			string id = this.Id;
			if (String.IsNullOrEmpty(id))
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
			}
			string requestUrl = string.Format("{0}/{1}/refunds", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Refund>>.ConvertFromJson(response);
		}
	}
}
