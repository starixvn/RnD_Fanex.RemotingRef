namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var kenoBetTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"KenoBetType?kenoBetTypeId={kenoBetTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var kenoBetTypeId = Fixture.Create<string>();
            var kenoBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = kenoBetTypeName });

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(kenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var kenoBetTypeId = Fixture.Create<string>();
            var defaultKenoBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                kenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableKenoBetTypeId(string unacceptableKenoBetTypeId)
        {
            // Arrange.
            var defaultKenoBetTypeName = Fixture.Create<string>();

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                unacceptableKenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert.
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableKenoBetTypeId = string.Empty;

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(unacceptableKenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }
    }
}