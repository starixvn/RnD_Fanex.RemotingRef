using PublicEnum.Language;
using RefClient.Factory;
using RefClient.Service.IService;
using RefDataModelStandard.Models;
using RefDataModelStandard.Models.Request;

namespace Fanex.RemotingRef.NetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddRefDataClient(this IApplicationBuilder app, IConfiguration configuration)
    {
        var redisList = configuration["TW.RemotingRef.Redis"].Split(',').ToList();
        var service = app.ApplicationServices.GetService<ILoggerService>();
        try
        {
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
        }
        catch(Exception ex)
        {
            service.Info(ex.Message);
        }
        
        return app;
    }
}