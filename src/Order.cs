using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Order : PayabbhiEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("merchant_order_id")]
		public string MerchantOrderId { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("payment_attempts")]
		public int PaymentAttempts { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/orders";

		public Order()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// List all orders
		/// </summary>
		/// <returns>List of orders</returns>
		/// <param name="options">Additional Options.</param>
		public PayabbhiList<Order> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Order>>.ConvertFromJson(response);
		}

		/// <summary>
		/// Retrieve an order using orderId.
		/// </summary>
		/// <returns>Order object</returns>
		/// <param name="id">The id of the order to retrieve.</param>
		public Order Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Order>.ConvertFromJson(response);
		}

		/// <summary>
		/// Create an order
		/// </summary>
		/// <returns>Order object</returns>
		/// <param name="options">Additional Options.</param>
		public Order Create(IDictionary<string, object> options)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Post, options);
			return Converter<Order>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all payments for an order
		/// </summary>
		/// <returns>List of payments for an order</returns>
		public PayabbhiList<Payment> Payments()
		{
			string id = this.Id;
			if (String.IsNullOrEmpty(id))
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
			}

			string requestUrl = string.Format("{0}/{1}/payments", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<PayabbhiList<Payment>>.ConvertFromJson(response);
		}
	}
}
