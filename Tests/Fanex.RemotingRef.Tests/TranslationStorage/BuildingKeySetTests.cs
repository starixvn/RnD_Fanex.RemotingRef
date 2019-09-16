namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public class BuildingKeySetTests
    {
        [Fact]
        public void BuiltKeyIsCombinationOfKeyValuePairsSeparatingByCurlyBrackets()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key1"] = "Value1",
                ["Key2"] = "Value2",
            };

            // Act.
            var actualResult = keySet.BuildKey();

            // Assert.
            const string expectedPattern = @"{([^\}]*)\}{([^\}]*)\}";

            Assert.Matches(expectedPattern, actualResult);
        }

        [Fact]
        public void KeyAndValueInBuiltKeySeparatingByColon()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key"] = "Value"
            };

            // Act.
            var actualResult = keySet.BuildKey();

            // Assert.
            const string expectedExistingString = "Key:Value";

            Assert.Contains(expectedExistingString, actualResult, StringComparison.InvariantCulture);
        }

        [Fact]
        public void BuiltKeyMeetsExpectation()
        {
            // Arrange.
            var keySet = new KeySet
            {
                ["Key1"] = "Value1",
                ["Key2"] = "Value2",
            };

            // Act.
            var actualResult = keySet.BuildKey();

            // Assert.
            const string expectedResult = "{Key1:Value1}{Key2:Value2}";

            Assert.Equal(expectedResult, actualResult);
        }
    }
}