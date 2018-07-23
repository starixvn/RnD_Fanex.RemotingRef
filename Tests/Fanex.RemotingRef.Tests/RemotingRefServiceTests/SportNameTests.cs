namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var sportTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"SportName?sportTypeId={sportTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var sportTypeId = Fixture.Create<string>();
            var sportTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = sportTypeName });

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(sportTypeName, actualSportName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var sportTypeId = Fixture.Create<string>();
            var defaultSportTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguageCode, defaultSportTypeName);

            // Assert.
            Assert.Equal(defaultSportTypeName, actualSportName);
        }

        [Fact]
        public void RetursEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var sportTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSportName);
        }
    }
}