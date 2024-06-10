using InfraNmodels.Interface;
using InfraNmodels.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace HelloAPIM
{
    public class Fnwelcome
    {
        private readonly ILogger<Fnwelcome> _logger;
        private readonly IAzureRedisCache _redisCache;
        public Fnwelcome(ILogger<Fnwelcome> logger, IAzureRedisCache _azRC)
        {
            _logger = logger;
            _redisCache = _azRC;
        }

        [Function("Welcome")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            //var stringContent = new StringContent(JsonObject.Parse(content).ToString(),System.Text.Encoding.UTF8);
            //JsonConvert.DeserializeObject<TemplateProperties>(content);
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            
            var isSet = _redisCache.SetCacheData("Kek01", content, expirationTime);
            string storedValue = string.Empty;
            if(isSet)
            {
                storedValue = _redisCache.GetCacheData<string>("Kek01");
            }
            //HttpClient httpClient = new HttpClient();
           // await httpClient.PostAsync("https://webhook.site/cc0de999-dc1f-4530-9c6a-8d25a5610b6d", stringContent);
           // var data = JsonObject.Parse(content);
            return new OkObjectResult($"Welcome to Azure Functions! your redis values is \"{storedValue}\", and now its removed.");
        }
    }
}
