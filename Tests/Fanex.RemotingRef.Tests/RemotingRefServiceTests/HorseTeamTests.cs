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
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"HorseTeamName?codeId={codeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
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
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var codeId = Fixture.Create<string>();
            var defaultHorseTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(
                codeId,
                TestingLanguageCode,
                defaultHorseTeamName);

            // Assert.
            Assert.Equal(defaultHorseTeamName, actualHorseTeamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableCodeId(string unacceptableCodeId)
        {
            // Arrange.
            var defaultHorseTeamName = Fixture.Create<string>();

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(
                unacceptableCodeId,
                TestingLanguageCode,
                defaultHorseTeamName);

            // Assert.
            Assert.Equal(defaultHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableCodeId = string.Empty;

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(unacceptableCodeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }
    }
}