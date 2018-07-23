namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var oddsTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"OddsTypeName?oddsTypeId={oddsTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var oddsTypeId = Fixture.Create<string>();
            var oddsTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = oddsTypeName });

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(oddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var oddsTypeId = Fixture.Create<string>();
            var defaultOddsTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                oddsTypeId,
                TestingLanguageCode,
                defaultOddsTypeName);

            // Assert.
            Assert.Equal(defaultOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var oddsTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }
    }
}