using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;

namespace Fanex.RemotingRef.Demo;

public static class WebOptimizerExtensions
{
    public static IServiceCollection AddAssetsBundle(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName).AddV8();


        services.AddWebOptimizer(pipeline =>
        {
            pipeline.CompileScssFiles();            
            pipeline.AddCssBundle("/assets/bundles/Site.css", "assets/styles/Site.css");
        });

        return services;
    }
}