using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Customer : PayabbhiEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

    [JsonProperty("email")]
		public string Email { get; set; }

    [JsonProperty("name")]
		public string Name { get; set; }

    [JsonProperty("contact_no")]
		public string ContactNo { get; set; }

		[JsonProperty("billing_address")]
		public object BillingAddress { get; set; }

		[JsonProperty("shipping_address")]
		public object ShippingAddress { get; set; }

		[JsonProperty("gstin")]
		public string Gstin { get; set; }

		[JsonProperty("subscriptions")]
		public PayabbhiList<Subscription> Subscriptions { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/customers";

		public Customer()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a customer object using customer id
		/// </summary>
		/// <returns>Customer Object</returns>
		/// <param name="id">The id of the customer to retrieve</param>
		public Customer Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Customer>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all customers.
		/// </summary>
		/// <returns>List of customers</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Customer> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Customer>>.ConvertFromJson(response);
		}

    /// <summary>
		/// Create a customer
		/// </summary>
		/// <returns>Customer object</returns>
		/// <param name="options">Additional Options.</param>
		public Customer Create(IDictionary<string, object> options)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Post, options);
			return Converter<Customer>.ConvertFromJson(response);
		}

		/// <summary>
		/// Update a customer
		/// </summary>
		/// <returns>Customer object</returns>
		/// <param name="options">Additional Options.</param>
		public Customer Update(IDictionary<string, object> options)
		{
			string id = this.Id;
			if (String.IsNullOrEmpty(id))
			{
				throw new Error.InvalidRequestError(Constants.Messages.InvalidCallError, null, null, HttpStatusCode.Unused);
			}
      string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Put, options);
			return Converter<Customer>.ConvertFromJson(response);
		}

	}
}
