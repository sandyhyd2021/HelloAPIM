using InfraNmodels.Implementation;
using InfraNmodels.Interface;
using InfraNmodels.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration((context, builder) =>
    {
        //var env = context.HostingEnvironment;
        //builder.SetBasePath(Directory.GetCurrentDirectory())
        //       .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
        //       .AddJsonFile($"local.settings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //       .AddEnvironmentVariables();
        if (context.HostingEnvironment.IsDevelopment())
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        }
        builder.AddEnvironmentVariables();
    })
    .ConfigureServices((cntx,services) =>
    {
        string connectionString = string.Empty;
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        //services.AddOptions<RedisSetting>().Configure<IConfiguration>((settings, config) =>
        //{
        //    config.GetSection(nameof(RedisSetting)).Bind(settings);
        //    connectionString = config.GetSection("RedisSetting:RedisUrl").Value!.ToString();
        //});

        var config = cntx.Configuration;
        connectionString = config.GetSection("RedisSetting:RedisUrl").Value!.ToString();
        services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer
        .Connect(connectionString));
        services.AddSingleton<IAzureRedisCache, AzureRedisCache>();
    })
    .Build();

host.Run();
