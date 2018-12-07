using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
	public class Product : PayabbhiEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("unit_label")]
		public string UnitLabel { get; set; }

		[JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

		readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/products";

		public Product()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Retrieves a product object using product id
		/// </summary>
		/// <returns>Product Object</returns>
		/// <param name="id">The id of the product to retrieve</param>
		public Product Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<Product>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all products.
		/// </summary>
		/// <returns>List of products</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<Product> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<Product>>.ConvertFromJson(response);
		}

		/// <summary>
		/// Create a product
		/// </summary>
		/// <returns>Product object</returns>
		/// <param name="options">Additional Options.</param>
		public Product Create(IDictionary<string, object> options)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Post, options);
			return Converter<Product>.ConvertFromJson(response);
		}

	}
}
