namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class HorseTeamTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var codeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"HorseTeamName?codeId={codeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var codeId = Fixture.Create<string>();
            var horseTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = horseTeamName });

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(horseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var codeId = Fixture.Create<string>();
            var defaultHorseTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguageCode, defaultHorseTeamName);

            // Assert.
            Assert.Equal(defaultHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var codeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }
    }
}