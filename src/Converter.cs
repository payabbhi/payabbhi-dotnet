using System.Reflection;
using Newtonsoft.Json;

namespace Payabbhi {
    public static class Converter<T> {
        /// <summary>
        /// Converts from json to a particular entity.
        /// </summary>
        /// <returns>Entity to which json is casted</returns>
        /// <param name="payabbhiResponse">Payabbhi response.</param>
        public static T ConvertFromJson (PayabbhiResponse payabbhiResponse) {
            var result = JsonConvert.DeserializeObject<T> (payabbhiResponse.ResponseJson);
            applyPayabbhiResponse (payabbhiResponse, result);
            return result;
        }

        static void applyPayabbhiResponse (PayabbhiResponse payabbhiResponse, object obj) {
            if (payabbhiResponse == null) return;

            PropertyInfo prop = obj.GetType ().GetProperty ("PayabbhiResponse", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite) {
                prop.SetValue (obj, payabbhiResponse, null);
            }
        }
    }
}