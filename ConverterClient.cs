using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DemoFunction
{
    public class ConverterClient
    {
        private Uri _baseUri;
        private string _code;

        public static ConverterClient CreateFromConfiguration()
        {
            string baseUri = ConfigurationUtils.GetEnvironmentVariable("ConverterUri");
            string code = ConfigurationUtils.GetEnvironmentVariable("ConverterKey");

            return new ConverterClient(new Uri(baseUri), code);
        }

        public ConverterClient(Uri baseUri, string code)
        {
            _baseUri = baseUri;
            _code = code;
        }

        public async Task<string> ConvertAsync(string templatePath, string content)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "text/plain");
            using (HttpClient client = new HttpClient())
            {
                var requestUri = new UriBuilder(_baseUri)
                {
                    Path = $"api/convert/{templatePath}",
                    Host = _baseUri.Host,
                    Scheme = _baseUri.Scheme,
                    Query = $"code={_code}"
                }.Uri;

                var response = await client.PostAsync(requestUri, httpContent);
                string responseContent = await response.Content.ReadAsStringAsync();

                return JObject.Parse(responseContent)?.SelectToken("$.fhirResource")?.ToString();
            }
            
        }
    }
}
