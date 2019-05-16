namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Delegates;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    [Collection("SharedContainer")]
    public sealed class ManagingRefDataTypeHandlersTests : IDisposable
    {
        private readonly Uri _serviceUri;
        private readonly RequestTranslationSetDelegate _requestHandler;
        private readonly SyncTranslationSetsDelegate _syncHandler;

        public ManagingRefDataTypeHandlersTests()
        {
            _serviceUri = new Uri("http://localhost:9999");
            _requestHandler = keySet => Task.FromResult(TranslationSet.FromDictionary(new Dictionary<string, string>()));
            _syncHandler = async (keySets) => await Task.FromResult(new Dictionary<string, TranslationSet>());
        }

        [Fact]
        public void ArgumentNullExceptionIsThrownOnceConnectingToNullUri()
        {
            // Arrange.
            Uri remotingRefServiceUri = null;

            // Act.
            var exception = Record.Exception(() => RefDataTypeContainer.ConnectTo(remotingRefServiceUri));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void NewFluentInstanceIsCreateOnceCallingConnectTo()
        {
            // Act.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Assert.
            Assert.NotNull(fluentInstance);
        }

        [Fact]
        public void RetrievingServiceUriBeforeCallingConnectToThrowsInvalidOperationException()
        {
            // Act.
            var exception = Record.Exception(() => RefDataTypeContainer.ServiceUri);

            // Assert.
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrievingServiceUriReturnsUriPassedToConnectTo()
        {
            // Arrange.
            RefDataTypeContainer
                .ConnectTo(_serviceUri)
                .FromApplication("AnApplicationName");

            // Act.
            var actualUri = RefDataTypeContainer.ServiceUri;

            // Assert.
            Assert.Equal(_serviceUri, actualUri);
        }

        [Fact]
        public void TryResolveDataTypeHandlerAlwaysReturnsRefDataTypeHandlerForEveryRefDataType()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.CountryName, _requestHandler, _syncHandler);

            // Assert.
            Assert.NotNull(handler);
        }

        [Fact]
        public void TryResolveDataTypeHandleReturnsRegisteredRefDataTypeHandlerIfAny()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(_serviceUri);

            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _requestHandler, _syncHandler);

            // Act.
            var observingHandler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _requestHandler, _syncHandler);

            // Assert.
            Assert.Same(handler, observingHandler);
        }

        [Fact]
        public void TryResolveDataTypeHandleUsesPreConfiguredRefDataTypeHandlerOptionsIfAny()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var observingHandler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _requestHandler, _syncHandler);

            // Assert.
            Assert.Equal(100_000, observingHandler.Options.NumberOfEntriesLimit);
            Assert.True(observingHandler.Options.CompactOnMemoryPressure);
        }

        [Fact]
        public void ArgumentNullExceptionIsThrownIfRegisterTakesANullRefDataTypeHandlerOptions()
        {
            // Arrange.
            RefDataTypeHandlerOptions options = null;
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var exception = Record.Exception(() => fluentInstance.Register(RefDataType.CountryName, options));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void RegisterOverwritesDefaultConfiguredOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var newOptions = new RefDataTypeHandlerOptions
            {
                NumberOfEntriesLimit = 1
            };

            fluentInstance.Register(RefDataType.TeamName, newOptions);

            // Assert.
            var observingHandler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _requestHandler, _syncHandler);
            var currentAffectedOptions = observingHandler.Options;

            Assert.NotEqual(100_000, currentAffectedOptions.NumberOfEntriesLimit);
        }

        [Fact]
        public void RegisterAddsOptionsToRegistedListIfNotExist()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var newOptions = new RefDataTypeHandlerOptions
            {
                NumberOfEntriesLimit = 1
            };

            fluentInstance.Register(RefDataType.SportName, newOptions);

            // Assert.
            var observingHandler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.SportName, _requestHandler, _syncHandler);
            var currentAffectedOptions = observingHandler.Options;

            Assert.Same(newOptions, currentAffectedOptions);
        }

        [Fact]
        public void SupportMultiLanguagesTurnsOnIsMultiLanguagesSupportedOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            RefDataTypeHandlerOptions.IsMultiLanguagesSupported = false;

            // Act.
            fluentInstance.SupportMultiLanguages();

            // Assert.
            Assert.True(RefDataTypeHandlerOptions.IsMultiLanguagesSupported);
        }

        [Fact]
        public void SupportMultiLanguagesReturnsCurrentFluentInstance()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var returnedInstance = fluentInstance.SupportMultiLanguages();

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        [Fact]
        public void SupportOnlyDefaultLanguageTurnsOffIsMultiLanguagesSupportedOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            RefDataTypeHandlerOptions.IsMultiLanguagesSupported = true;

            // Act.
            fluentInstance.SupportOnlyDefaultLanguage();

            // Assert.
            Assert.False(RefDataTypeHandlerOptions.IsMultiLanguagesSupported);
        }

        [Fact]
        public void SupportOnlyDefaultLanguageReturnsCurrentFluentInstance()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var returnedInstance = fluentInstance.SupportOnlyDefaultLanguage();

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        [Fact]
        public void RegisterApplicationNameAppendsApplicationNameWithVersion()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);
            var applicationName = "an application name";

            // Act.
            fluentInstance.FromApplication(applicationName);

            // Assert.
            var expectedApplicationName = $"{applicationName}-{RefDataTypeContainer.GetVersion()}";

            Assert.Equal(expectedApplicationName, RefDataTypeHandlerOptions.ApplicationName);
        }

        [Fact]
        public void RegisterApplicationNameReturnsCurrentFluentInstance()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var returnedInstance = fluentInstance.FromApplication("an application name");

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        [Fact]
        public void RetrievingServiceUriBeforeRegisteringApplicationNameThrowsInvalidOperationException()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var exception = Record.Exception(() => RefDataTypeContainer.ServiceUri);

            // Assert.
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RegisterSupportedLanguagesOverwritesSupportedLanguageCodesOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);
            var supportLanguages = new[] { "first", "second" };

            // Act.
            fluentInstance
                .SupportMultiLanguages()
                .RegisterSupportedLanguages(supportLanguages);

            // Assert.
            Assert.Equal(supportLanguages, RefDataTypeHandlerOptions.SupportedLanguageCodes);
        }

        [Fact]
        public void RegisterSupportedLanguagesReturnsCurrentFluentInstance()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(_serviceUri);

            // Act.
            var returnedInstance = fluentInstance.RegisterSupportedLanguages("first", "second");

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        public void Dispose()
            => RefDataTypeContainer.Reset();
    }
}