namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void ByBetTypeId_SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ByBetTypeId_ReturnEmptyStringIfServiceReturnsNull()
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
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&refNo={refNo}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ByBetTypeIdAndRefNo_ReturnProvidedDefaultValueIfServiceReturnsNull()
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

        [Fact]
        public void ByBetTypeIdAndRefNo_ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var refNo = Fixture.Create<long>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
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
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ByBetTypeIdAndResourceId_ReturnProvidedDefaultValueIfServiceReturnsNull()
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
            string expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}&resourceId={resourceId}&refNo={refNo}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ByBetTypeIdResourceIdAndRefNo_ReturnProvidedDefaultValueIfServiceReturnsNull()
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
    }
}