# Fanex.RemotingRef

Fanex.RemotingRef is a wrapper of TW RemotingRef 2.0. Its main purpose is to support .NET Standard and reduce the number of dependencies sticked with TW RemotingRef 2.0.

As for the technical overview, check out this [page](https://gitlab.nexcel.vn/RnD/Fanex.RemotingRef/wikis/Technical-Overview)

As for APIs supported by Fanex.RemotingRef, check out this [page](https://gitlab.nexcel.vn/RnD/Fanex.RemotingRef/wikis/apis)

## Install

NuGet package is available [here](http://nuget.nexdev.net/packages/Fanex.RemotingRef/)

## Get Started

### ASP.NET Core

1. After install NuGet package, in the `ConfigureServices` method of `Startup.cs`, connect to Service URL and register the `RemotingRefService`

    ```csharp
        RefDataTypeContainer
            .ConnectTo(new Uri(Configuration["Fanex.RemotingRef.Service.Url"]))
            .FromApplication("Your Application Name");
        services.AddMvc();
        services.Configure<IConfiguration>(Configuration);
        services.AddSingleton<IRemotingRefService, RemotingRefService>();
    ```

2. Configure RemotingRef Service url in `appsettings.json`. Refer [here](#fanexremotingrefservice-url) to get Fanex.RemotingRef.Service URL

    ```json
        {
            "Fanex.RemotingRef.Service.Url": "http://ha.nexdev.net:7258/Main/"
        }
    ```

3. Use `RemotingRefService` instance in anywhere you want to use

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

### ASP.NET MVC

1. After install NuGet package, in the `Application_Start` method of `Global.asax.cs`, connect to the Service as

    ```csharp
        RefDataTypeContainer
            .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
            .FromApplication("Your Application Name");
    ```

2. Configure RemotingRef Service URL in `web.config`.  Refer [here](#fanexremotingrefservice-url) to get Fanex.RemotingRef.Service URL

    ```config
        <appSettings>
            <add key="Fanex.RemotingRef.Service.Url" value="http://ha.nexdev.net:7258/Main/" />
        </appSettings>

    ```

3. Use `RemotingRefService` in anywhere you want to use

    ```csharp
        public string GetBetTypeName(string betTypeId, string languageCode)
        {
            var betTypeName =  new RemotingRefService().GetBetTypeName(betTypeId, languageCode);

            return betTypeName;
        }
    ```

## Configuration Information

### Fanex.RemotingRef.Service URL

| Environment | Dev                           | Master                          |
| ----------- | ----------------------------- | ------------------------------- |
| LOCAL       | http://ha.nexdev.net:7258/V1/ | http://ha.nexdev.net:7258/Main/ |
| SIT         | http://172.16.4.105:6207/V1/  | http://172.16.4.105:6207/Main/  |
| UAT         | http://10.40.40.248:7207/V1/  | http://10.40.40.248:7207/Main/  |
| PRO         |                               | http://10.40.40.47:7258/        |
| MANILA      |                               | http://10.2.98.2:7258/          |

## Configuration & Customization

- Fanex.RemotingRef allow you to customize cached data types and supported languages in your site. All customized methods are supported in class `RefDataTypeContainer`. For example:

    ```csharp
         RefDataTypeContainer
            .ConnectTo(new Uri(Configuration["RemotingRefService"]))
            .FromApplication("Fanex.RemotingRef.Demo")
            .RegisterSupportedLanguages("en-US", "ja-JP")
            .Register(
                RefDataType.BetTypeName,
                new RefDataTypeHandlerOptions
                {
                    SlidingTimeout = TimeSpan.FromMilliseconds(40)
                });
    ```

### RefDataTypeContainer

| **Method**                                                                | **Description**                                                                         |
| ------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- |
| ConnectTo(**Uri** remotingRefServiceUri)                                  | Connect to RemotingRef Service with Url                                                 |
| FromApplication(**string** applicationName)                               | Provide an application name for statistics purpose                                      |
| SupportMultiLanguages()                                                   | Supports 8 default languages (en-US, zh-TW, zh-CN, ja-JP, th-TH, ko-KR, vi-VN, zh-Hans) |
| SupportOnlyDefaultLanguage()                                              | Only support English (en-US)                                                            |
| RegisterSupportedLanguages(**params string[]** languageCodes)             | Support optional languages                                                              |
| Register(**RefDataType** dataType, **RefDataTypeHandlerOptions** options) | Register caching data type with customized *RefDataTypeHandlerOptions*                  |

### RefDataTypeHandlerOptions

| **Options**             | **Description**                                                                                                                                                                                                                                                                                          | **Type** | **Default Value** |
| ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ----------------- |
| SyncBatchSize           | Number of item of DataType for each sync time                                                                                                                                                                                                                                                            | int      | 100               |
| SlidingTimeout          | Cache sliding time out                                                                                                                                                                                                                                                                                   | TimeSpan | 10 minutes        |
| RenewalTime             | Time to sync data from Ref Service                                                                                                                                                                                                                                                                       | Timespan | 40 seconds        |
| NumberOfEntriesLimit    | The limited number of cached items for each *DataType* in local storage. When cached items amount is greater than *NumberOfEntriesLimit*, Fanex.RemotingRef will remove rarely accessed items for adding new item to local cache. The number of removed items is based on **CompactionPercentage** value | int      | 1000              |
| CompactionPercentage    | The percentage of item amount that will be removed when high memory or reaching *NumberOfEntriesLimit*                                                                                                                                                                                                   | double   | 0.1 (10%)         |
| CompactOnMemoryPressure | Enable cleaning cached items when Gargabe Collector runs                                                                                                                                                                                                                                                 | bool     | false             |
| ExpirationScanFrequency | Time to scan expired cached items                                                                                                                                                                                                                                                                        | Timespan | 1 minute          |

## Special Notes

### There is some functions have been renamed

| **Old**              | **New**              |
| -------------------- | -------------------- |
| GetKenoBettype       | GetKenoBetType       |
| GetFastMarketsChoise | GetFastMarketsChoice |
| GetLotteryBettype    | GetLotteryBetType    |

### The Language Code parameter, which is passed to Fanex.RemotingRef, is standard language code (en-US, zh-TW, zh-CN, ja-JP, ...)