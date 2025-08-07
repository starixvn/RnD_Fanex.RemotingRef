namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Linq;
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
        }

        [Fact]
        public void SendRequestForEachSelectionId()
        {
            // Arrange.
            // Setup response for each selectionId
            for (var index = 0; index < _selectionIds.Length; index++)
            {
                var response = new HttpResponseMessage();
                response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

                ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + _selectionIds[index]);
                HttpClient
                    .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                    .Returns(response);
            }

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
        public void SendRequestWithCorrectParametersForEachSelectionId()
        {
            // Arrange.
            // Setup response for each selectionId
            for (var index = 0; index < _selectionIds.Length; index++)
            {
                var response = new HttpResponseMessage();
                response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

                ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + _selectionIds[index]);
                HttpClient
                    .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                    .Returns(response);
            }

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
            // Setup response for each selectionId
            for (var index = 0; index < _selectionIds.Length; index++)
            {
                var response = new HttpResponseMessage();
                response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

                ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + _selectionIds[index]);
                HttpClient
                    .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                    .Returns(response);
            }

            // Act.
            var betBuilderSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(_selectionIds, TestingLanguageCode);

            // Assert.
            Assert.Empty(betBuilderSelectionNames);
        }

        [Fact]
        public void DoNotSendRequestForInvalidSelectionId()
        {
            // Arrange.
            const int invalidSelectionId = 0;

            var validSelectionId = Fixture.Create<int>();
            var selectionIds = new[] { validSelectionId, invalidSelectionId };

            // Setup response only for valid selectionId
            var response = new HttpResponseMessage();
            response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, RelativePath + validSelectionId);
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(response);

            // Act.
            RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguageCode);

            // Assert.
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
            Assert.Empty(actualSelectioNames);
        }
    }
}