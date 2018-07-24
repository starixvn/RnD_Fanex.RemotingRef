namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLeague3rds = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _league3rdIds;

        public LeagueName3rdTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _league3rdIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLeague3rds; counter++)
            {
                var league3rdId = Fixture.Create<string>();

                _league3rdIds.Add(league3rdId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["league3rdId"] = league3rdId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetLeagueName3rdOfRefDataHelper()
        {
            // Act.
            RefSyncController.LeagueName3rd(_formData);

            // Assert.
            RefData.Received().GetLeagueName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachLeague3rdId()
        {
            // Act.
            RefSyncController.LeagueName3rd(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetLeagueName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LeagueName3rd(_formData);

            // Assert.
            foreach (var league3rdId in _league3rdIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetLeagueName3rd(Arg.Is(league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdLeagueNames();

            // Act.
            var actualResult = RefSyncController.LeagueName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdLeagueNames(true, null);

            // Act.
            var actualResult = RefSyncController.LeagueName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdLeagueNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.LeagueName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSynced3rdLeagueNames()
            => FakeSynced3rdLeagueNames(false, null);

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSynced3rdLeagueNames(
            bool requireSpecialValue,
            string specialValue)
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
                var league3rdId = _league3rdIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var leagueNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLeagueName3rd(Arg.Is(league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
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