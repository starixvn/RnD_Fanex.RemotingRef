namespace Fanex.RemotingRef.NetCore
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.AspNetCore;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.RabbitMQ;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Fanex.RemotingRef.NetCore.Utilities.RefData;
    using RefClient.Factory;
    using RefClient.Service;
    using RefClient.Service.IService;
    using RefDataModelStandard.Models;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRefDataClient(this IServiceCollection services, IConfiguration configuration)
        {
            var redisList = configuration["TW.RemotingRef.Redis"].Split(',').ToList();
            RefClientFactory.InitializeConfig(new ClientConfigModel
            {
                RefDataWebApiUrl = new RefDataWebApiUrl
                {
                    Url = configuration["TW.RemotingRef.API"]
                },
                RefDataLocalRedisConfiguration = new RedisConfiguration
                {
                    Host = redisList,
                    DatabaseId = int.Parse(configuration["TW.RemotingRef.DatabaseId"]),
                }
            });

            return services;
        }

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
                    .AddSingleton<ILoggerService, ILoggerService>();

            ExceptionHandler
                .UseConfig(new LoggingConfiguration()
                .GetHttpHeaderInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo));

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IBetRefClientService>(serviceProvider =>
                RefClientFactory.GetBetRefClientService());
            services.AddSingleton<IRefDataClient>(serviceProvider =>
                RefClientFactory.GetRefDataClient());
            services.AddSingleton<IThirdPartyRefClientService>(serviceProvider =>
                RefClientFactory.GetThirdPartyRefClientService());

            services.AddSingleton<IRefDataHelper, RefDataHelper>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ITeam3rdService, ITeam3rdService>();
            services.AddSingleton<ILeague3rdService, ILeague3rdService>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();

            return services;
        }
    }
}