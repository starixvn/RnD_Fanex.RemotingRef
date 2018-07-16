namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefControllerBaseTests
    {
        private readonly string _betTypeId;
        private readonly string _resourceId;
        private readonly long? _refNo;

        public BetTypeNameTests()
        {
            _betTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();
        }

        [Fact]
        public void CallGetBetTypeNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetTypeName(_betTypeId, _resourceId, _refNo, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetBetTypeName(
                    Arg.Is(_betTypeId),
                    Arg.Any<string>(),
                    Arg.Is(_resourceId),
                    Arg.Is(_refNo));
        }

        [Fact]
        public void SendCorrectArgumentsToGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            FakeBetTypeNameById();

            // Act.
            var actualBetTypeNames = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualBetTypeNames.Keys);
        }

        [Fact]
        public void ReturnResultWithDataGotFromGetBetTypeNameOfRefDataHelperAsValues()
        {
            // Arrange.
            var expectedBetTypeNames = FakeBetTypeNameById();

            // Act.
            var actualBetTypeNames = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetTypeNames, actualBetTypeNames);
        }

        private Dictionary<string, string> FakeBetTypeNameById()
        {
            var expectedBetTypeNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo))
                    .Returns(betTypeNameInSpecificLanguage);
                expectedBetTypeNames.Add(standardLanguageCode, betTypeNameInSpecificLanguage);
            }

            return expectedBetTypeNames;
        }
    }
}