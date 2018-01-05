namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public class RefDataTypeHandlerGetSetTests
    {
        private const string TestingLanguageCode = "cn-TW";

        private readonly IRefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly LocalStorage _storage;
        private readonly TranslationSet _predefinedTranslationSet;

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
        }

        [Fact]
        public void GettingUnexistingReturnsValueCreatedByValueFactory()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };

            _options.ValueFactory = (parameters) => _predefinedTranslationSet;

            // Act.
            var actualResult = _handler.Get(keySet, TestingLanguageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[TestingLanguageCode];

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ReturningNullValueFactoryMakesGetReturnNull()
        {
            // Arrange.
            var unexistingKeySet = new KeySet
            {
                ["UnexistingTranslation"] = "UnexistingTranslation"
            };

            _options.ValueFactory = (parameters) => null;

            // Act.
            var actualResult = _handler.Get(unexistingKeySet, TestingLanguageCode);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void RetrievingUndefinedLanguageCodeReturnsEnglish()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };

            _options.ValueFactory = (parameters) => _predefinedTranslationSet;

            // Act.
            var undefinedLanguageCode = "abc";
            var actualResult = _handler.Get(keySet, undefinedLanguageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[RefDataTypeHandlerOptions.DefaultLanguage];

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmptyLanguageCodeMakesGetReturnEnglish(string languageCode)
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };

            _options.ValueFactory = (parameters) => _predefinedTranslationSet;

            // Act.
            var actualResult = _handler.Get(keySet, languageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[RefDataTypeHandlerOptions.DefaultLanguage];

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TranslationSetCreatedByValueFactoryIsStoredInStorage()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };

            _options.ValueFactory = (parameters) => _predefinedTranslationSet;

            // Act.
            _handler.Get(keySet, null);

            // Assert.
            var found = _storage.TryGet(keySet.BuildKey(), out _);

            Assert.True(found);
        }

        [Fact]
        public void TranslationSetDoesNotContainAnyLanguageCodeMakesGetReturnNull()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            _options.ValueFactory = (parameters) => value;

            // Act.
            var actualResult = _handler.Get(keySet, null);

            // Assert.
            Assert.Null(actualResult);
        }
    }
}