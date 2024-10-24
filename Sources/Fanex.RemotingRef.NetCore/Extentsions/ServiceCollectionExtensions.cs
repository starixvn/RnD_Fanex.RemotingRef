using Fanex.Caching;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.RabbitMQ;
using Fanex.RemotingRef.NetCore.Services;
using Fanex.RemotingRef.NetCore.Utilities.RefData;

namespace Fanex.RemotingRef.NetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFanexLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var logEngines = new List<ILogEngine>
            {
                new RabbitMQLogging(configuration["Fanex.Logging:RabbitMQConnectionStrings"])
            };
        LogManager
                .SetDefaultLogCategory(configuration["Fanex.Logging:DefaultCategory"])
                .Use(logEngines);

        services.AddSingleton(Logger.Log)
                .AddSingleton<ILoggerService, LoggerService>();

        ExceptionHandler
            .UseConfig(new LoggingConfiguration()
            .GetHttpHeaderInfo()
            .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IRefDataHelper, RefDataHelper>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<ITeam3rdService, Team3rdService>();
        services.AddSingleton<ILeague3rdService, League3rdService>();
        services.AddSingleton<IBettypeService, BettypeService>();
        services.AddSingleton<ISportService, SportService>();
        services.AddSingleton<IRefResourceService, RefResourceService>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        
        return services;
    }

    public static IApplicationBuilder UseFanexData(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
    {
        var connectionStrings = configuration
            .GetSection("ConnectionStrings")
            .GetChildren()
            .ToDictionary(
                connection => connection.Key,
                connection => new ConnectionConfiguration(connection.Key, connection.Value),
                StringComparer.OrdinalIgnoreCase);

        var resourcePath = Path.Combine("App_Data", "SPNames.xml");
        DbSettingProviderManager
            .StartNewSession()
            .WithSqlServer(resourcePath)
            .Use(connectionStrings)
            .Run();

        return app;
    }
}