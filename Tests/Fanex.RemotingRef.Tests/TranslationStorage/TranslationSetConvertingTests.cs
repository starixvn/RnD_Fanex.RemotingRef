namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public class TranslationSetConvertingTests
    {
        [Fact]
        public void ArgumentNullExceptionIsThrownWhenInputIsNull()
        {
            // Arrange.
            IDictionary<string, object> dictionary = null;

            // Act.
            Exception exception = Record.Exception(() => TranslationSet.FromDictionary(dictionary));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void NumberOfItemsAddedToTranslationSetMeetsNumberOfItemsInInput()
        {
            // Arrange.
            var dictionary = new Dictionary<string, string>
            {
                ["Key1"] = "Value1",
                ["Key2"] = "Value2"
            };

            // Assert.
            var actualResult = TranslationSet.FromDictionary(dictionary);

            // Assert.
            Assert.Equal(dictionary.Count, actualResult.Count);
        }

        [Fact]
        public void ItemsAddedToTranslationSetMatchesContentOfInput()
        {
            // Arrange.
            var dictionary = new Dictionary<string, string>
            {
                ["Key"] = "Value"
            };

            // Assert.
            var actualResult = TranslationSet.FromDictionary(dictionary);

            // Assert.
            var expectedPair = dictionary.ElementAt(0);
            var observingPair = actualResult.ElementAt(0);

            Assert.Equal(expectedPair.Key, observingPair.Key);
            Assert.Equal(expectedPair.Value, observingPair.Value);
        }
    }
}