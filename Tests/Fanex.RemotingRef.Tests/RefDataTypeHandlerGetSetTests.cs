namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class RefDataTypeHandlerGetSetTests : IDisposable
    {
        private const string TestingLanguageCode = "zh-TW";

        private readonly IRefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly TranslationSet _predefinedTranslationSet;

#pragma warning disable S3459 // Unassigned members should be removed
        private readonly KeySet _keySet;
#pragma warning restore S3459 // Unassigned members should be removed

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
            _options.RequestHandler = (parameters) => Task.FromResult(_predefinedTranslationSet);
            (_keySet, _) = LocalStorageExtensions.CreateKeySet();
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
            _options.RequestHandler = (parameters) => Task.FromResult(default(TranslationSet));

            // Act.
            var actualResult = _handler.Get(_keySet, TestingLanguageCode);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void RetrievingUndefinedLanguageCodeReturnsEnglish()
        {
            // Arrange.
            const string undefinedLanguageCode = "Undefined Language Code";

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

            _options.RequestHandler = (parameters) => Task.FromResult(value);

            // Act.
            var actualResult = _handler.Get(_keySet, null);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void RetryIfFailedToGetDataAtTheFirstTime()
        {
            // Arrange.
            var firstTime = true;

            _options.RequestHandler = parameters =>
            {
                if (firstTime)
                {
                    firstTime = false;

                    return null;
                }

                return Task.FromResult(_predefinedTranslationSet);
            };

            // Act.
            var actualResult = _handler.Get(_keySet, TestingLanguageCode);

            // Assert.
            var expectedResult = _predefinedTranslationSet[TestingLanguageCode];

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NoExceptionRethrownIfFailedToGetDataAtTheFirstTime()
        {
            // Arrange.
            var firstTime = true;

            _options.RequestHandler = parameters =>
            {
                if (firstTime)
                {
                    firstTime = false;

                    throw new InvalidOperationException();
                }

                return Task.FromResult(_predefinedTranslationSet);
            };

            // Act.
            var exception = Record.Exception(() => _handler.Get(_keySet, TestingLanguageCode));

            // Assert.
            Assert.Null(exception);
        }

        [Fact]
        public void ExceptionRethrownAtTheLastFailedRetry()
        {
            // Arrange.
            var callCounter = 0;
            var expectedException = new InvalidOperationException();

            _options.RequestHandler = parameters =>
            {
                callCounter++;

                if (_options.NumberOfRetries == callCounter)
                {
                    throw expectedException;
                }

                return null;
            };

            // Act.
            var actualException = Record.Exception(() => _handler.Get(_keySet, TestingLanguageCode));

            // Assert.
            Assert.Equal(expectedException, actualException);
        }

        [Fact]
        public void NumberOfRequestHandlerCallsDoesNotExceedConfiguredNumberOfRetries()
        {
            // Arrange.
            var callCounter = 0;

            _options.RequestHandler = parameters =>
            {
                callCounter++;

                return Task.FromResult(default(TranslationSet));
            };

            // Act.
            _handler.Get(_keySet, TestingLanguageCode);

            // Assert.
            Assert.Equal(_options.NumberOfRetries, callCounter);
        }

        [Fact]
        public void ReturningNullAfterNumberOfRetriesFailed()
        {
            // Arrange.
            _options.RequestHandler = parameters => Task.FromResult(default(TranslationSet));

            // Act.
            var actualResult = _handler.Get(_keySet, TestingLanguageCode);

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