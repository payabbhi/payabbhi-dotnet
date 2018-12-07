using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Transfer : PayabbhiEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("source_id")]
		public string SourceId { get; set; }

		[JsonProperty("recipient_id")]
		public string RecipientId { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("fees")]
		public int Fees { get; set; }

		[JsonProperty("gst")]
		public int Gst { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/transfers";

		public Transfer()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a transfer object using transfer id
		/// </summary>
		/// <returns>Transfer Object</returns>
		/// <param name="id">The id of the transfer to retrieve</param>
		public Transfer Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Transfer>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all transfers.
		/// </summary>
		/// <returns>List of transfers</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Transfer> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Transfer>>.ConvertFromJson(response);
		}

	}
}
