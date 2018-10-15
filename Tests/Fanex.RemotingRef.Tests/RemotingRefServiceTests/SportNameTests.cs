namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RemotingRefServiceTestBase
    {
        private readonly string _sportTypeId;

        public SportNameTests()
            => _sportTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"SportName?sportTypeId={_sportTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var sportTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = sportTypeName });

            // Act.
            var actualSportName = RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(sportTypeName, actualSportName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultSportTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode, defaultSportTypeName);

            // Assert.
            Assert.Equal(defaultSportTypeName, actualSportName);
        }

        [Fact]
        public void RetursEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSportName);
        }
    }
}