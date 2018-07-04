namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetCombinationTypeComboSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTeam = _fixture.Create<string>();
            var betCheck = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();

            // Act.
            _remotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"CombinationTypeComboParlay?betTeam={betTeam}&betCheck={betCheck}&handicap1={handicap1}&handicap2={handicap2}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetCombinationTypeComboReturnsValueGotFromService()
        {
            // Arrange.
            var betTeam = _fixture.Create<string>();
            var betCheck = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();
            var combinationTypes = _fixture.Create<string[]>();

            _serializer
                .Deserialize<Dictionary<string, string[]>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string[]> { { TestingLanguageCode, combinationTypes } });

            // Act.
            var actualCombinationTypes = _remotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(combinationTypes, actualCombinationTypes);
        }

        [Fact]
        public void GetCombinationTypeComboReturnsNullIfServiceReturnsNull()
        {
            // Arrange.
            var betTeam = _fixture.Create<string>();
            var betCheck = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCombinationTypes = _remotingRefService.GetCombinationTypeCombo(
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