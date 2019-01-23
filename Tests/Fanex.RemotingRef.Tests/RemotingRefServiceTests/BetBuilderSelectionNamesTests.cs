namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Net.Http;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetBuilderSelectionNamesTests : RemotingRefServiceTestBase
    {
        private const string RelativePath = "BetBuilderSelectionName?selectionId=";

        private readonly int[] _selectionIds;

        public BetBuilderSelectionNamesTests()
        {
            _selectionIds = Fixture.Create<int[]>();

            HttpClient
                .GetAsync(Arg.Is<string>(requestRelativePath => requestRelativePath.Contains(RelativePath)))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestForEachSelectionId()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(_selectionIds, TestingLanguageCode);

            // Assert.
            HttpClient.Received(_selectionIds.Length);
        }

        [Fact]
        public void SendRequestWithCorrectParametersForEachSelectionId()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(_selectionIds, TestingLanguageCode);

            // Assert.
            foreach (var selectionId in _selectionIds)
            {
                ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + selectionId);

                HttpClient
                    .Received()
                    .GetAsync(Arg.Is(ExpectedRequestRelativePath));
            }
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var selectionNames = new string[_selectionIds.Length];

            for (var index = 0; index < _selectionIds.Length; index++)
            {
                var selectionName = Fixture.Create<string>();
                var response = new HttpResponseMessage
                {
                    Content = FakeResponseContent(SerializeTranslationSet(selectionName))
                };

                ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + _selectionIds[index]);
                HttpClient
                    .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                    .Returns(response);

                selectionNames[index] = selectionName;
            }

            // Act.
            var actualSelectioNames = RemotingRefService.GetBetBuilderSelectionNames(_selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Equal(selectionNames, actualSelectioNames);
        }

        [Fact]
        public void ReturnEmptyWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualSelectioNames = RemotingRefService.GetBetBuilderSelectionNames(_selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSelectioNames);
        }

        [Fact]
        public void DoNotSendRequestForInvalidSelectionId()
        {
            // Arrange.
            var validSelectionId = Fixture.Create<int>();
            var invalidSelectionId = 0;
            var selectionIds = new[] { validSelectionId, invalidSelectionId };

            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + validSelectionId);
            HttpClient
                .Received(1)
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
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