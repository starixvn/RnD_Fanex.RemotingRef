namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class HorseTeamTests : RemotingRefServiceTestBase
    {
        private readonly string _codeId;

        public HorseTeamTests()
        {
            _codeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"HorseTeamName?codeId={_codeId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var horseTeamName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(horseTeamName));

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(_codeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(horseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultHorseTeamName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(
                _codeId,
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