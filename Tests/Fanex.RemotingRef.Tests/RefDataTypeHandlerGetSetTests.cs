namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class RefDataTypeHandlerGetSetTests : IDisposable
    {
        private const string TestingLanguageCode = "zh-TW";

        private readonly IRefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly KeySet _keySet;
        private readonly TranslationSet _predefinedTranslationSet;

        private LocalStorage _storage;

        public RefDataTypeHandlerGetSetTests()
        {
            _options = new RefDataTypeHandlerOptions();
            _storage = new LocalStorage(_options, new TestClock());
            _handler = new RefDataTypeHandler(_options, _storage);
            _predefinedTranslationSet = TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [TestingLanguageCode] = "Chinese",
                [RefDataTypeHandlerOptions.DefaultLanguage] = "English"
            });
            _options.RequestHandler = (parameters) => _predefinedTranslationSet;

            _keySet = new KeySet
            {
                ["Key"] = "Value"
            };
        }

        [Fact]
        public void GettingUncachedTranslationSetReturnsValueCreatedByRequestHandler()
        {
            // Act.
            var actualResult = _handler.Get(_keySet, TestingLanguageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[TestingLanguageCode];

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ReturningNullRequestHandlerMakesGetReturnNull()
        {
            // Arrange.
            var unexistingKeySet = new KeySet
            {
                ["UnexistingTranslation"] = "UnexistingTranslation"
            };

            _options.RequestHandler = (parameters) => null;

            // Act.
            var actualResult = _handler.Get(unexistingKeySet, TestingLanguageCode);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void RetrievingUndefinedLanguageCodeReturnsEnglish()
        {
            // Arrange.
            var undefinedLanguageCode = "Undefined Language Code";

            // Act.
            var actualResult = _handler.Get(_keySet, undefinedLanguageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[RefDataTypeHandlerOptions.DefaultLanguage];

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmptyLanguageCodeMakesGetReturnEnglish(string languageCode)
        {
            // Act.
            var actualResult = _handler.Get(_keySet, languageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[RefDataTypeHandlerOptions.DefaultLanguage];

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TranslationSetReturnedByRequestHandlerIsStoredInStorage()
        {
            // Act.
            _handler.Get(_keySet, null);

            // Assert.
            var found = _storage.TryGet(_keySet.BuildKey(), out _);

            Assert.True(found);
        }

        [Fact]
        public void TranslationSetDoesNotContainAnyLanguageCodeMakesGetReturnNull()
        {
            // Arrange.
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            _options.RequestHandler = (parameters) => value;

            // Act.
            var actualResult = _handler.Get(_keySet, null);

            // Assert.
            Assert.Null(actualResult);
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}