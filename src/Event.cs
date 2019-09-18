using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Payabbhi {
    public class Event : PayabbhiEntity {
        [JsonProperty ("id")]
        public string Id { get; set; }

        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("type")]
        public string Type { get; set; }

        [JsonProperty ("environment")]
        public string Environment { get; set; }

        [JsonProperty ("data")]
        public object Data { get; set; }

        [JsonProperty ("created_at")]
        public int CreatedAt { get; set; }

        readonly HttpClient httpClient;
        string relativeUrl = "/api/v1/events";

        public Event () {
            httpClient = new HttpClient ();
        }

        /// <summary>
        /// Retrieves an event object using event id
        /// </summary>
        /// <returns>Event Object</returns>
        /// <param name="id">The id of the event to retrieve</param>
        public Event Retrieve (string id) {
            string requestUrl = string.Format ("{0}/{1}", relativeUrl, id);
            var response = httpClient.Request (requestUrl, HttpMethod.Get, null);
            return Converter<Event>.ConvertFromJson (response);
        }

        /// <summary>
        /// List all events.
        /// </summary>
        /// <returns>List of events</returns>
        /// <param name="options">Additional Options</param>
        public PayabbhiList<Event> All (IDictionary<string, object> options = null) {
            var response = httpClient.Request (relativeUrl, HttpMethod.Get, options);
            return Converter<PayabbhiList<Event>>.ConvertFromJson (response);
        }

    }
}