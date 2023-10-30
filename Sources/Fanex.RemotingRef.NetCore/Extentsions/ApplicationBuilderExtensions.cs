using RefClient.Factory;
using RefDataModelStandard.Models;

namespace Fanex.RemotingRef.NetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddRefDataClient(this IApplicationBuilder app, IConfiguration configuration)
    {
        //var redisList = configuration["TW.RemotingRef.Redis"].Split(',').ToList();
        RefClientFactory.InitializeConfig(new ClientConfigModel
        {
            RefDataWebApiUrl = new RefDataWebApiUrl
            {
                Url = configuration["TW.RemotingRef.API"]
            },
            RefDataLocalRedisConfiguration = new RedisConfiguration
            {
                Host = new List<string>
                {
                   configuration["TW.RemotingRef.Redis"] ?? string.Empty,
                },
                DatabaseId = 3,
            }
        });

        return app;
    }
}