using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
namespace Payabbhi {
    [JsonObject]
    public class PayabbhiList<T> : PayabbhiEntity, IEnumerable<T> {
        [JsonProperty ("object")]
        public string Object { get; set; }

        [JsonProperty ("data")]
        public List<T> Data { get; set; }

        [JsonProperty ("total_count")]
        public int TotalCount { get; set; }

        public IEnumerator<T> GetEnumerator () {
            return Data.GetEnumerator ();
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator () {
            return Data.GetEnumerator ();
        }
    }
}