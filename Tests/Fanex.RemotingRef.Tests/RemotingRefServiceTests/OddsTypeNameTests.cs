namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RemotingRefServiceTestBase
    {
        private readonly string _oddsTypeId;

        public OddsTypeNameTests()
            => _oddsTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetOddsTypeName(_oddsTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"OddsTypeName?oddsTypeId={_oddsTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var oddsTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = oddsTypeName });

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(_oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(oddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultOddsTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                _oddsTypeId,
                TestingLanguageCode,
                defaultOddsTypeName);

            // Assert.
            Assert.Equal(defaultOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(_oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }
    }
}