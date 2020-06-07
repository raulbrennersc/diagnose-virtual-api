using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Helpers
{
    public static class HttpRequestHelper
    {
        public static T MakeJsonRequest<T>(HttpClient client, string url, object body)
        {
            try
            {
                var jsonObject = JsonConvert.SerializeObject(body, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = (client.PostAsync(url, content).Result).Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch
            {
                return default;
            }
        }
    }
}
