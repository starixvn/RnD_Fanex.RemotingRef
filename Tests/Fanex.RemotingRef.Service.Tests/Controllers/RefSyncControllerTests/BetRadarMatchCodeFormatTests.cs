namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingMatchCodes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _matchCodes;

        public BetRadarMatchCodeFormatTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _matchCodes = new List<string>();

            for (var counter = 0; counter < NumberOfTestingMatchCodes; counter++)
            {
                var matchCode = Fixture.Create<string>();

                _matchCodes.Add(matchCode);
                _parameterSets.Add(new ParameterSet
                {
                    ["matchCode"] = matchCode
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetBetRadarMatchCodeFormatOfRefDataHelper()
        {
            // Act.
            RefSyncController.BetRadarMatchCodeFormat(_formData);

            // Assert.
            RefData.Received().GetBetRadarMatchCodeFormat(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachMatchCode()
        {
            // Act.
            RefSyncController.BetRadarMatchCodeFormat(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetBetRadarMatchCodeFormat(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.BetRadarMatchCodeFormat(_formData);

            // Assert.
            foreach (var matchCode in _matchCodes)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetBetRadarMatchCodeFormat(Arg.Is(matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetRadarMatchCodeFormats();

            // Act.
            var actualResult = RefSyncController.BetRadarMatchCodeFormat(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<ParameterSet, Dictionary<string, string>> FakeSyncedBetRadarMatchCodeFormats()
        {
            var expectedSyncedMatchFormats = new Dictionary<ParameterSet, Dictionary<string, string>>();

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var matchFormatInMultilanguages = new Dictionary<string, string>();
                var matchCode = _matchCodes[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var matchFormatInSpecificLanguage = Fixture.Create<string>();

                    RefData
                        .GetBetRadarMatchCodeFormat(Arg.Is(matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(matchFormatInSpecificLanguage);
                    matchFormatInMultilanguages.Add(standardLanguageCode, matchFormatInSpecificLanguage);
                }

                expectedSyncedMatchFormats.Add(_parameterSets[counter], matchFormatInMultilanguages);
            }

            return expectedSyncedMatchFormats;
        }
    }
}