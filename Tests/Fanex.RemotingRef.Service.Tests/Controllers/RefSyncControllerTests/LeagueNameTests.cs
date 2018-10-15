namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLeagues = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _leagueIds;

        public LeagueNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _leagueIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLeagues; counter++)
            {
                var leagueId = Fixture.Create<string>();

                _leagueIds.Add(leagueId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(leagueId)] = leagueId
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
            RefData
                .Received()
                .GetLeagueName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachLeagueId()
        {
            // Act.
            RefSyncController.LeagueName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetLeagueName(Arg.Any<string>(), Arg.Any<string>());
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
                    RefData
                        .Received()
                        .GetLeagueName(Arg.Is(leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
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

        private SyncedTranslationResultSet FakeSyncedLeagueNames()
            => FakeSyncedLeagueNames(false, null);

        private SyncedTranslationResultSet FakeSyncedLeagueNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedLeagueNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var leagueNameInMultilanguages = new TranslationSet();
                var leagueId = _leagueIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var leagueNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLeagueName(Arg.Is(leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(leagueNameInSpecificLanguage);

                    leagueNameInMultilanguages.Add(standardLanguageCode, leagueNameInSpecificLanguage);
                }

                if (ValidateTextSet(leagueNameInMultilanguages))
                {
                    expectedSyncedLeagueNames.Add(_parameterSets[counter], leagueNameInMultilanguages);
                }
            }

            return expectedSyncedLeagueNames;
        }
    }
}