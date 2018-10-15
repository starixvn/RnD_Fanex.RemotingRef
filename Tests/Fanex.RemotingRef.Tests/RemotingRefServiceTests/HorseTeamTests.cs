namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class HorseTeamTests : RemotingRefServiceTestBase
    {
        private readonly string _codeId;

        public HorseTeamTests()
            => _codeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"HorseTeamName?codeId={_codeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var horseTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = horseTeamName });

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(horseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultHorseTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode, defaultHorseTeamName);

            // Assert.
            Assert.Equal(defaultHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }
    }
}