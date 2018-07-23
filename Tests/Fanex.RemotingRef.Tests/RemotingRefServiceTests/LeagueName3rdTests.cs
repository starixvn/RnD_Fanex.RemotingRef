namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var league3rdId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName3rd?league3rdId={league3rdId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var league3rdId = Fixture.Create<string>();
            var leagueName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = leagueName3rd });

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var league3rdId = Fixture.Create<string>();
            var defaultLeagueName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode, defaultLeagueName3rd);

            // Assert.
            Assert.Equal(defaultLeagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var league3rdId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName3rd);
        }
    }
}