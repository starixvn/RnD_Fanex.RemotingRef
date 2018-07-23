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
            string expectedUrl = string.Format(RequestUrlPattern, $"KenoBetType?kenoBetTypeId={kenoBetTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
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

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var kenoBetTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }
    }
}