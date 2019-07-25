using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi
{
    public class BeneficiaryAccount : PayabbhiEntity
    {
        [JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("object")]
		public string Object { get; set; }

        [JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("contact_no")]
		public string ContactNo { get; set; }

        [JsonProperty("email_id")]
		public string EmailId { get; set; }

		[JsonProperty("bussiness_name")]
		public string BussinessName { get; set; }

        [JsonProperty("business_entity_type")]
		public string BusinessEntityType { get; set; }

		[JsonProperty("beneficiary_name")]
		public string BeneficiaryName { get; set; }

        [JsonProperty("ifsc")]
        public string Ifsc { get; set; }

        [JsonProperty("bank_account_number")]
        public string AccountNo { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("notes")]
		public object Notes { get; set; }

		[JsonProperty("created_at")]
		public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
		string relativeUrl = "/api/v1/beneficiaryaccounts";

		public BeneficiaryAccount()
		{
			httpClient = new HttpClient();
		}

        /// <summary>
		/// Retrieves a beneficiary object using beneficiary id
		/// </summary>
		/// <returns>Beneficiary Object</returns>
		/// <param name="id">The id of the beneficiary to retrieve</param>
		public BeneficiaryAccount Retrieve(string id)
		{
			string requestUrl = string.Format("{0}/{1}", relativeUrl, id);
			var response = httpClient.Request(requestUrl, HttpMethod.Get, null);
			return Converter<BeneficiaryAccount>.ConvertFromJson(response);
		}

		/// <summary>
		/// List all beneficiaries.
		/// </summary>
		/// <returns>List of beneficiaries</returns>
		/// <param name="options">Additional Options</param>
		public PayabbhiList<BeneficiaryAccount> All(IDictionary<string, object> options = null)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Get, options);
			return Converter<PayabbhiList<BeneficiaryAccount>>.ConvertFromJson(response);
		}

        /// <summary>
		/// Creates a new beneficiary
		/// </summary>
		/// <returns>Beneficiary Object</returns>
		/// <param name="options">Addition Options</param>
        public PaymentLink Create(IDictionary<string, object> options)
		{
			var response = httpClient.Request(relativeUrl, HttpMethod.Post, options);
			return Converter<PaymentLink>.ConvertFromJson(response);
		}
    }
}