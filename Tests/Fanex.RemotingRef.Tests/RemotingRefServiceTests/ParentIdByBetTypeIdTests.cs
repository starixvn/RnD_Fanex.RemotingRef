namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _defaultParentTypeId;

        public ParentIdByBetTypeIdTests()
        {
            _betTypeId = Fixture.Create<string>();
            _defaultParentTypeId = Fixture.Create<string>();
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ParentIdByBetTypeId?betTypeId={_betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var parentTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = parentTypeId });

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(parentTypeId, actualParentTypeId);
        }

        [Fact]
        public void ReturnDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(_defaultParentTypeId, actualParentTypeId);
        }
    }
}