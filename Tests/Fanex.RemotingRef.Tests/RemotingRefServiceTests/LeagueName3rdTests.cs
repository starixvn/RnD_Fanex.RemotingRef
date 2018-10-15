namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RemotingRefServiceTestBase
    {
        private readonly string _league3rdId;

        public LeagueName3rdTests()
            => _league3rdId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName3rd?league3rdId={_league3rdId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = leagueName3rd });

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultLeagueName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode, defaultLeagueName3rd);

            // Assert.
            Assert.Equal(defaultLeagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName3rd);
        }
    }
}