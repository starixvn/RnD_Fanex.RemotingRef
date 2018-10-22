namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _defaultParentTypeId;

        public ParentIdByBetTypeIdTests()
            => _defaultParentTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetParentIdByBetTypeId(betTypeId, _defaultParentTypeId);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"ParentIdByBetTypeId?betTypeId={betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var parentTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = parentTypeId });

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(parentTypeId, actualParentTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(_defaultParentTypeId, actualParentTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(unacceptableBetTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(_defaultParentTypeId, actualParentTypeId);
        }
    }
}