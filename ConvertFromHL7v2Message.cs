using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace DemoFunction
{
    public static class ConvertFromHL7v2Message
    {
        [FunctionName("ConvertHL7v2Message")]
        public static async Task<IActionResult> ConvertHL7v2Message(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);
            ConverterClient client = ConverterClient.CreateFromConfiguration();

            return new OkObjectResult(await client.ConvertAsync(ConfigurationUtils.GetEnvironmentVariable("ConverterTemplate"), requestBody));
        }
    }
}
