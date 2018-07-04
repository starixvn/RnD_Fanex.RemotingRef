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
        public void GetBetTypeNameWithBetTypeIdOnlySendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetBetTypeNameWithBetTypeIdOnlyReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var betTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, betTypeName } });

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithBetTypeIdOnlyReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithRefNoSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();

            // Act.
            _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&refNo={refNo}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetBetTypeNameWithRefNoReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();
            var betTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, betTypeName } });

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithRefNoAndDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();
            var defaultBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo,
                defaultBetTypeName);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithRefNoAndNoDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var defaultBetTypeName = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var defaultBetTypeName = _fixture.Create<string>();
            var betTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, betTypeName } });

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdAndDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var defaultBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdAndRefNoSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();
            var defaultBetTypeName = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}&refNo={refNo}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdAndRefNoReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();
            var defaultBetTypeName = _fixture.Create<string>();
            var betTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, betTypeName } });

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetTypeNameWithResourceIdAndRefNoAndDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var refNo = _fixture.Create<long>();
            var defaultBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = _remotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }
    }
}