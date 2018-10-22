namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
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
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetCombinationTypeCombo(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"CombinationTypeComboParlay?betTeam={_betTeam}&betCheck={_betCheck}&handicap1={_handicap1}&handicap2={_handicap2}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var combinationTypes = Fixture.Create<string[]>();

            Serializer
                .Deserialize<Dictionary<string, string[]>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string[]> { [TestingLanguageCode] = combinationTypes });

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
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

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