# Fanex.RemotingRef

Fanex.RemotingRef is a wrapper of TW RemotingRef 2.0. Its main purpose is to support .NET Standard and reduce the number of dependencies sticked with TW RemotingRef 2.0. The overview [can be found here](https://gitlab.nexcel.vn/RnD/Fanex.RemotingRef/wikis/overview)

# Install

NuGet package is available [here](http://nuget.nexdev.net/packages/RemotingRef/)

# Get Started

#### ASP.NET Core

1. After install NuGet package, in the *ConfigureServices* method of *Startup.cs*, connect to Service url and register the RemotingRefService


    ```csharp
        RefDataTypeContainer.ConnectTo(new Uri(Configuration["RemotingRefService"]));
        services.AddMvc();
        services.Configure<IConfiguration>(Configuration);
        services.AddSingleton<IRemotingRefService, RemotingRefService>();
    ```
    
2. Configure RemotingRef Service url in *appsettings.json*
    ```json
        {
            "RemotingRefService": "http://10.19.10.144:9999/"
        }
    ```

3. Use *RemotingRefService* instance in where you want to use

    ```csharp
        private readonly IRemotingRefService _remotingRefService;
    
        public AnonymousController(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }
        
        public IActionResult BetTypeName([FromQuery]string betTypeId, [FromQuery]string languageCode)
        {
            var betTypeName = _remotingRefService.GetBetTypeName(betTypeId, languageCode);

            return Ok(betTypeName);
        }
    ```
    
#### ASP.NET MVC

1. After install NuGet package, in the *Application_Start* method of *Global.asax.cs*, connect to Service url


    ```csharp
        RefDataTypeContainer.ConnectTo(new Uri(ConfigurationManager.AppSettings["RemotingRefService"]));
    ```
    
2. Configure RemotingRef Service url in *web.config*
    ```config
        <appSettings>
            <add key="RemotingRefService" value="http://10.18.200.103:7258/Main/" />
        </appSettings>

    ```
    
3. Use *RemotingRefService* in where you want to use

    ```csharp
        public string GetBetTypeName(string betTypeId, string languageCode)
        {
            var betTypeName =  new RemotingRefService().GetBetTypeName(betTypeId, languageCode);

            return betTypeName;
        }
    ```
    
# Configuration & Customization
