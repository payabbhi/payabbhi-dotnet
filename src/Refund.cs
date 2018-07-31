using System.Collections.Generic;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Refund : PayabbhiEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("amount")]
		public int Amount { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("payment_id")]
		public string PaymentId { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/refunds";

		public Refund()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a refund object using refund id.
		/// </summary>
		/// <returns>Refund object</returns>
		/// <param name="id">The id of the refund to retrieve</param>
		public Refund Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Refund>.ConvertFromJson(response);
		}

		/// <summary>
		/// Create a refund
		/// </summary>
		/// <returns>Refund object</returns>
		/// <param name="paymentId">PaymentId</param>
		/// <param name="options">Additional options</param>
		public Refund Create(string paymentId, IDictionary<string, object> options = null)
		{
			string requestUrl = string.Format("/api/v1/payments/{0}/refunds", paymentId);
			var response = httpClient.Request(requestUrl, HttpMethod.Post, options);
			return Converter<Refund>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all refunds
		/// </summary>
		/// <returns>List of refunds</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Refund> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Refund>>.ConvertFromJson(response);
		}
	}
}
