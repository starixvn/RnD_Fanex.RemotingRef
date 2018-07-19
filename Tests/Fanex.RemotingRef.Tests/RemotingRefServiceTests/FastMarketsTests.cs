namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();

            // Act.
            RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarketsChoice?betTypeId={betTypeId}&betTeam={betTeam}&handicap1={handicap1}&handicap2={handicap2}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();
            var fastMarketsChoice = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = fastMarketsChoice });

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(fastMarketsChoice, actualFastMarketsChoice);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();
            var handicap1 = Fixture.Create<int>();
            var handicap2 = Fixture.Create<int>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }
    }
}