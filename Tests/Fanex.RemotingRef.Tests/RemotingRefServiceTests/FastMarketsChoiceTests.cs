namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsChoiceTests : RemotingRefServiceTestBase
    {
        private readonly int _handicap1;
        private readonly int _handicap2;

        public FastMarketsChoiceTests()
        {
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"FastMarketsChoice?betTypeId={betTypeId}&betTeam={betTeam}&handicap1={_handicap1}&handicap2={_handicap2}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();
            var fastMarketsChoice = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = fastMarketsChoice });

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(fastMarketsChoice, actualFastMarketsChoice);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTeam = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnEmptyStringForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Arrange.
            var betTeam = Fixture.Create<string>();

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                unacceptableBetTypeId,
                betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnEmptyStringForNullOrEmptyBetTeam(string invalidBetTeam)
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                invalidBetTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }
    }
}