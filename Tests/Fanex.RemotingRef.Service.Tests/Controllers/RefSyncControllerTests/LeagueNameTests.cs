namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLeagues = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _leagueIds;

        public LeagueNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _leagueIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLeagues; counter++)
            {
                var leagueId = Fixture.Create<string>();

                _leagueIds.Add(leagueId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["leagueId"] = leagueId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetLeagueNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.LeagueName(_formData);

            // Assert.
            RefData.Received().GetLeagueName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachLeagueId()
        {
            // Act.
            RefSyncController.LeagueName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetLeagueName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LeagueName(_formData);

            // Assert.
            foreach (var leagueId in _leagueIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetLeagueName(Arg.Is(leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedLeagueNames();

            // Act.
            var actualResult = RefSyncController.LeagueName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLeagueNames(true, null);

            // Act.
            var actualResult = RefSyncController.LeagueName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLeagueNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.LeagueName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedLeagueNames(
            bool requireSpecialValue = false,
            string specialValue = null)
        {
            var expectedSyncedLeagueNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var leagueNameInMultilanguages = new Dictionary<string, string>();
                var leagueId = _leagueIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var leagueNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLeagueName(Arg.Is(leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(leagueNameInSpecificLanguage);
                    leagueNameInMultilanguages.Add(standardLanguageCode, leagueNameInSpecificLanguage);
                }

                if (leagueNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedLeagueNames.Add(_parameterSets[counter], leagueNameInMultilanguages);
                }
            }

            return expectedSyncedLeagueNames;
        }
    }
}