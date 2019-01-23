namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CombinationTypeComboTests : RemotingRefServiceTestBase
    {
        private readonly string _betTeam;
        private readonly string _betCheck;
        private readonly int _handicap1;
        private readonly int _handicap2;

        public CombinationTypeComboTests()
        {
            _betTeam = Fixture.Create<string>();
            _betCheck = Fixture.Create<string>();
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"CombinationTypeComboParlay?betTeam={_betTeam}&betCheck={_betCheck}&handicap1={_handicap1}&handicap2={_handicap2}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string[]>()));

            // Act.
            RemotingRefService.GetCombinationTypeCombo(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var combinationTypes = Fixture.Create<string[]>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(combinationTypes));

            // Act.
            var actualCombinationTypes = RemotingRefService.GetCombinationTypeCombo(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(combinationTypes, actualCombinationTypes);
        }

        [Fact]
        public void ReturnNullWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualCombinationTypes = RemotingRefService.GetCombinationTypeCombo(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Null(actualCombinationTypes);
        }
    }
}