namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetOddsTypeNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var oddsTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"OddsTypeName?oddsTypeId={oddsTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetOddsTypeNameReturnsValueGotFromService()
        {
            // Arrange.
            var oddsTypeId = _fixture.Create<string>();
            var oddsTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, oddsTypeName } });

            // Act.
            var actualOddsTypeName = _remotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(oddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void GetOddsTypeNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var oddsTypeId = _fixture.Create<string>();
            var defaultOddsTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = _remotingRefService.GetOddsTypeName(
                oddsTypeId,
                TestingLanguageCode,
                defaultOddsTypeName);

            // Assert.
            Assert.Equal(defaultOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void GetOddsTypeNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var oddsTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualOddsTypeName = _remotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }
    }
}