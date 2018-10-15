namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedBetTypeNameTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;

        public DisplayedBetTypeNameTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void ByBetTypeId_SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetDisplayedBetTypeName(_betTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedBetTypeName?betTypeId={_betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ByBetTypeId_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(_betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeId_ReturnEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(_betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var refNo = Fixture.Create<long>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedBetTypeName?betTypeId={_betTypeId}&refNo={refNo}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var refNo = Fixture.Create<long>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var refNo = Fixture.Create<long>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedBetTypeName?betTypeId={_betTypeId}&resourceId={resourceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdAndResourceId_ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
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
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            string expectedUrl = string.Format(
                RequestUrlPattern,
                $"DisplayedBetTypeName?betTypeId={_betTypeId}&resourceId={resourceId}&refNo={refNo}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayedBetTypeName });

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ByBetTypeIdResourceIdAndRefNo_ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                resourceId,
                refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }
    }
}