namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public class TranslationSetConvertingTests
    {
        [Fact]
        public void NullIsReturnedWhenInputIsNull()
        {
            // Arrange.
            IDictionary<string, object> dictionary = null;

            // Act.
            var actualResult = TranslationSet.FromDictionary(dictionary);

            // Assert.
            Assert.True(actualResult.Count() == 0);
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

            // Act.
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

            // Act.
            var actualResult = TranslationSet.FromDictionary(dictionary);

            // Assert.
            var expectedPair = dictionary.ElementAt(0);
            var observingPair = actualResult.ElementAt(0);

            Assert.Equal(expectedPair.Key, observingPair.Key);
            Assert.Equal(expectedPair.Value, observingPair.Value);
        }
    }
}