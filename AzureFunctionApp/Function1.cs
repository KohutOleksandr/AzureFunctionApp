using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Generic;

namespace AzureFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("JSON Request Data: " + RequestData(req));

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. \n {RequestData(req)}";

            return new OkObjectResult(responseMessage);
        }

        public static string Proverb()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 11);

            Dictionary<int, string> phrase = new Dictionary<int, string>
            {
                {1, "Good can never grow out of evil"},
                {2, "He laughs best who laughs last." },
                {3, "You can take a horse to the water, but you cannot make him drink." },
                {4, "Salt water and absence wash away love." },
                {5, "Many a true word is spoken in jest." },
                {6, "Night brings counsel." },
                {7, "Practice makes perfect" },
                {8, "Truth is stranger than fiction" },
                {9, "Two is company, but three is none. " },
                {10, "We never know the value of water till the well is dry. " }
            };
            return phrase[randomNumber];
        }

        public static string RequestData(HttpRequest req)
        {
            string proverb = Proverb();
            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();
            string method = req.Method;
            string url = req.GetDisplayUrl();
            var path = req.Path;
            var host = req.Host;
            string protocol = req.Protocol;
            var body = req.Body.ToString();
            var headers = req.Headers;
            var queryParams = req.Query;

            var requestData = new
            {
                Proverb = proverb,
                DayOfWeek = dayOfWeek,
                HttpMethod = method,
                RequestUrl = url,
                Host = host,
                Path = path,
                Protocol = protocol,
                Body = body,
                Headers = headers,
                QueryParameters = queryParams,

            };

            return JsonConvert.SerializeObject(requestData); 
        }
    }
    
}
