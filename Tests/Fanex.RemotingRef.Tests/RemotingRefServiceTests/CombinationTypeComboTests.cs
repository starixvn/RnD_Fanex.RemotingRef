namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CombinationTypeComboTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTeam = Fixture.Create<string>();
            var betCheck = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();

            // Act.
            RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"CombinationTypeComboParlay?betTeam={betTeam}&betCheck={betCheck}&handicap1={handicap1}&handicap2={handicap2}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTeam = Fixture.Create<string>();
            var betCheck = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();
            var combinationTypes = Fixture.Create<string[]>();

            Serializer
                .Deserialize<Dictionary<string, string[]>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string[]> { [TestingLanguageCode] = combinationTypes });

            // Act.
            var actualCombinationTypes = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(combinationTypes, actualCombinationTypes);
        }

        [Fact]
        public void ReturnNullIfServiceReturnsNull()
        {
            // Arrange.
            var betTeam = Fixture.Create<string>();
            var betCheck = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCombinationTypes = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Null(actualCombinationTypes);
        }
    }
}