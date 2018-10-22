namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void ByBetTypeId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"BetTypeName?betTypeId={betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ByBetTypeId_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var betTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = betTypeName });

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeId_ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeId_ReturnEmptyString(string unacceptableBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(unacceptableBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();

            // Act.
            RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetTypeName?betTypeId={betTypeId}&refNo={refNo}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var betTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = betTypeName });

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                refNo,
                defaultBetTypeName);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeIdAndRefNo_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                refNo,
                defaultBetTypeName);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableBetTypeId = string.Empty;
            var refNo = Fixture.Create<long>();

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                refNo);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultBetTypeName = Fixture.Create<string>();
            var betTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = betTypeName });

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeIdAndResourceId_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}&refNo={refNo}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();
            var betTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = betTypeName });

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeIdResourceIdAndRefNo_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }
    }
}