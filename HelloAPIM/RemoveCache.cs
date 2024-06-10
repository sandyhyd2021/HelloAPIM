using InfraNmodels.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HelloAPIM
{
    public class RemoveCache
    {
        private readonly ILogger<RemoveCache> _logger;
        private readonly IAzureRedisCache _redisCache;
        public RemoveCache(ILogger<RemoveCache> logger, IAzureRedisCache redisCache)
        {
            _logger = logger;
            _redisCache = redisCache;
        }

        [Function("RemoveCache")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            _redisCache.RemoveData("Kek01");
            return new OkObjectResult("Cache removed.");
        }
    }
}
