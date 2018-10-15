namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _betTeam;
        private readonly int _handicap1;
        private readonly int _handicap2;

        public FastMarketsChoiceTests()
        {
            _betTypeId = Fixture.Create<string>();
            _betTeam = Fixture.Create<string>();
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(
                RequestUrlPattern,
                $"FastMarketsChoice?betTypeId={_betTypeId}&betTeam={_betTeam}&handicap1={_handicap1}&handicap2={_handicap2}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var fastMarketsChoice = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = fastMarketsChoice });

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(fastMarketsChoice, actualFastMarketsChoice);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }
    }
}