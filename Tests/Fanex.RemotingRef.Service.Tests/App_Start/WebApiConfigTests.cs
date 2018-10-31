namespace Fanex.RemotingRef.Service.Tests
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ModelBinding.Binders;
    using Fanex.RemotingRef.Service.Filters;
    using Xunit;

    public sealed class WebApiConfigTests : IDisposable
    {
        private readonly HttpConfiguration _config;

        public WebApiConfigTests()
            => _config = new HttpConfiguration();

        [Fact]
        public void AddGlobalExceptionFilterAttribute()
        {
            // Act.
            WebApiConfig.Register(_config);

            // Assert.
            Assert.IsType<GlobalExceptionFilterAttribute>(_config.Filters.ElementAt(0).Instance);
        }

        [Fact]
        public void InsertSimpleModelBinderProviderAsFirstModelBinderProvider()
        {
            // Act.
            WebApiConfig.Register(_config);

            // Assert.
            var modelBinderProviders = _config.Services.GetServices(typeof(ModelBinderProvider));

            Assert.IsType<SimpleModelBinderProvider>(modelBinderProviders.ElementAt(0));
        }

        public void Dispose()
            => _config.Dispose();
    }
}