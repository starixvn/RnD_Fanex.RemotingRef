namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Delegates;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public class ManagingRefDataTypeHandlersTests
    {
        private readonly RequestTranslationSetDelegate _valueFactory;
        private readonly SyncTranslationSetsDelegate _syncDataHandler;

        public ManagingRefDataTypeHandlersTests()
        {
            _valueFactory = keySet => TranslationSet.FromDictionary(new Dictionary<string, string>());
            _syncDataHandler = async (keySets) => await Task.FromResult(new Dictionary<string, TranslationSet>());
        }

        [Fact]
        public void ArgumentNullExceptionIsThrownOnceConnectingToNullUri()
        {
            // Arrange.
            Uri remotingRefServiceUri = null;

            // Act.
            Exception exception = Record.Exception(() => RefDataTypeContainer.ConnectTo(remotingRefServiceUri));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void NewFluentInstanceIsCreateOnceCallingConnectTo()
        {
            // Arrange.
            var remotingRefServiceUri = new Uri("localhost:9999");

            // Act.
            var fluentInstance = RefDataTypeContainer.ConnectTo(remotingRefServiceUri);

            // Assert.
            Assert.NotNull(fluentInstance);
        }

        [Fact]
        public void RetrievingServiceUriBeforeCallingConnectToThrowsInvalidOperationException()
        {
            // Arrange.
            RefDataTypeContainer.Reset();

            // Act.
            Exception exception = Record.Exception(() => RefDataTypeContainer.ServiceUri);

            // Assert.
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrievingServiceUriReturnsUriPassedToConnectTo()
        {
            // Arrange.
            var remotingRefServiceUri = new Uri("localhost:9999");

            RefDataTypeContainer.ConnectTo(remotingRefServiceUri);

            // Act.
            var actualUri = RefDataTypeContainer.ServiceUri;

            // Assert.
            Assert.Equal(remotingRefServiceUri, actualUri);
        }

        [Fact]
        public void TryResolveDataTypeHandlerAlwaysReturnsRefDataTypeHandlerForEveryRefDataType()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.CountryName, _valueFactory, _syncDataHandler);

            // Assert.
            Assert.NotNull(handler);
        }

        [Fact]
        public void TryResolveDataTypeHandleReturnsRegisteredRefDataTypeHandlerIfAny()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _valueFactory, _syncDataHandler);

            // Act.
            var observingHandler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _valueFactory, _syncDataHandler);

            // Assert.
            Assert.Same(handler, observingHandler);
        }

        [Fact]
        public void TryResolveDataTypeHandleUsesPreConfiguredRefDataTypeHandlerOptionsIfAny()
        {
            // Arrange.
            RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _valueFactory, _syncDataHandler);

            // Assert.
            Assert.Equal(100_000, handler.Options.NumberOfEntriesLimit);
            Assert.True(handler.Options.CompactOnMemoryPressure);
        }

        [Fact]
        public void ArgumentNullExceptionIsThrownIfRegisterTakesANullRefDataTypeHandlerOptions()
        {
            // Arrange.
            RefDataTypeHandlerOptions options = null;
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            Exception exception = Record.Exception(() => fluentInstance.Register(RefDataType.CountryName, options));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void RegisterOverwritesDefaultConfiguredOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var newOptions = new RefDataTypeHandlerOptions
            {
                NumberOfEntriesLimit = 1
            };

            fluentInstance.Register(RefDataType.TeamName, newOptions);

            // Assert.
            var currentAffectedOptions = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.TeamName, _valueFactory, _syncDataHandler).Options;

            Assert.NotEqual(100_000, currentAffectedOptions.NumberOfEntriesLimit);
        }

        [Fact]
        public void RegisterAddsOptionsToRegistedListIfNotExist()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var newOptions = new RefDataTypeHandlerOptions
            {
                NumberOfEntriesLimit = 1
            };

            fluentInstance.Register(RefDataType.SportName, newOptions);

            // Assert.
            var currentAffectedOptions = RefDataTypeContainer.TryResolveDataTypeHandler(RefDataType.SportName, _valueFactory, _syncDataHandler).Options;

            Assert.Same(newOptions, currentAffectedOptions);
        }

        [Fact]
        public void SupportMultiLanguagesTurnsOnIsMultiLanguagesSupportedOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

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
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var returnedInstance = fluentInstance.SupportMultiLanguages();

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        [Fact]
        public void SupportOnlyDefaultLanguageTurnsOffIsMultiLanguagesSupportedOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

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
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var returnedInstance = fluentInstance.SupportOnlyDefaultLanguage();

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }

        [Fact]
        public void RegisterSupportedLanguagesOverwritesSupportedLanguageCodesOfRefDataTypeHandlerOptions()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));
            var supportLanguages = new[] { "first", "second" };

            // Act.
            fluentInstance.RegisterSupportedLanguages(supportLanguages);

            // Assert.
            Assert.Equal(supportLanguages, RefDataTypeHandlerOptions.SupportedLanguageCodes);
        }

        [Fact]
        public void RegisterSupportedLanguagesReturnsCurrentFluentInstance()
        {
            // Arrange.
            var fluentInstance = RefDataTypeContainer.ConnectTo(new Uri("localhost:9999"));

            // Act.
            var returnedInstance = fluentInstance.RegisterSupportedLanguages("first", "second");

            // Assert.
            Assert.Same(fluentInstance, returnedInstance);
        }
    }
}