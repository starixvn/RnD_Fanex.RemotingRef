namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var defaultParentTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ParentIdByBetTypeId?betTypeId={betTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var parentTypeId = Fixture.Create<string>();
            var defaultParentTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = parentTypeId });

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            Assert.Equal(parentTypeId, actualParentTypeId);
        }

        [Fact]
        public void ReturnDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var defaultParentTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            Assert.Equal(defaultParentTypeId, actualParentTypeId);
        }
    }
}