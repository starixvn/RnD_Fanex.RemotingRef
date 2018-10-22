namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedBetTypeNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void ByBetTypeId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"DisplayedBetTypeName?betTypeId={betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ByBetTypeId_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
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
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeId_ReturnEmptyString(string unacceptableBetTypeId)
        {
            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(unacceptableBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"DisplayedBetTypeName?betTypeId={betTypeId}&refNo={refNo}");

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
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeIdAndRefNo_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableBetTypeId = string.Empty;
            var refNo = Fixture.Create<long>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"DisplayedBetTypeName?betTypeId={betTypeId}&resourceId={resourceId}");

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
            var defaultDisplayedBetTypeName = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ByUnacceptableBetTypeIdAndResourceId_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"DisplayedBetTypeName?betTypeId={betTypeId}&resourceId={resourceId}&refNo={refNo}");

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
            var defaultDisplayedBetTypeName = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
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
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }
    }
}