namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetBuilderSelectionNamesTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestForEachSelectionId()
        {
            // Arrange.
            var selectionIds = Fixture.Create<int[]>();

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            HttpClient.Received(selectionIds.Length);
        }

        [Fact]
        public void SendRequestWithCorrectParametersForEachSelectionId()
        {
            // Arrange.
            var selectionIds = Fixture.Create<int[]>();

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            foreach (var selectionId in selectionIds)
            {
                var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"BetBuilderSelectionName?selectionId={selectionId}");

                HttpClient
                    .Received()
                    .GetAsync(Arg.Is(expectedRequestRelativePath));
            }
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var selectionIds = Fixture.Create<int[]>();
            var selectionNames = Fixture.Create<string[]>();

            Serializer
                .Deserialize<Dictionary<string, string[]>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string[]> { [TestingLanguageCode] = selectionNames });

            // Act.
            var actualSelectioNames = RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Equal(selectionNames, actualSelectioNames);
        }

        [Fact]
        public void ReturnEmptyWhenServiceReturnsNull()
        {
            // Arrange.
            var selectionIds = Fixture.Create<int[]>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSelectioNames = RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSelectioNames);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new int[0])]
        public void ReturnNullForNullOrEmptySelectionIds(int[] selectionIds)
        {
            // Act.
            var actualSelectioNames = RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Null(actualSelectioNames);
        }
    }
}