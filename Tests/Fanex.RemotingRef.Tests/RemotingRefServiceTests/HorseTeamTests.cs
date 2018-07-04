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
        public void GetHorseTeamNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var codeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"HorseTeamName?codeId={codeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetHorseTeamNameReturnsValueGotFromService()
        {
            // Arrange.
            var codeId = _fixture.Create<string>();
            var horseTeamName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, horseTeamName } });

            // Act.
            var actualHorseTeamName = _remotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(horseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void GetHorseTeamNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var codeId = _fixture.Create<string>();
            var defaultHorseTeamName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = _remotingRefService.GetHorseTeamName(codeId, TestingLanguageCode, defaultHorseTeamName);

            // Assert.
            Assert.Equal(defaultHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void GetHorseTeamNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var codeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = _remotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }
    }
}