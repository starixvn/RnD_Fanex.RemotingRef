namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RemotingRefServiceTestBase
    {
        private readonly string _kenoBetTypeId;

        public KenoBetTypeTests()
            => _kenoBetTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetKenoBetType(_kenoBetTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"KenoBetType?kenoBetTypeId={_kenoBetTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var kenoBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = kenoBetTypeName });

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(_kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(kenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultKenoBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                _kenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(_kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }
    }
}