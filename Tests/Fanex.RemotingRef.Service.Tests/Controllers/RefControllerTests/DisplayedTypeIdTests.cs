namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RefControllerBaseTests
    {
        private readonly string _betTypeId;

        public DisplayedTypeIdTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void CallGetDisplayedTypeIdOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            RefData.Received(1).GetDisplayedTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void SendCorrentArgumentsToRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            RefData.Received().GetDisplayedTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void ReturnDataGotFromGetDisplayedTypeIdOfRefDataHelper()
        {
            // Arrange.
            var expectedDisplayedTypeIds = FakeDisplayedTypeId();

            // Act.
            var actualDisplayedTypeIds = RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(expectedDisplayedTypeIds, actualDisplayedTypeIds);
        }

        private Dictionary<string, string> FakeDisplayedTypeId()
        {
            var displayedTypeId = Fixture.Create<string>();

            RefData.GetDisplayedTypeId(Arg.Is(_betTypeId)).Returns(displayedTypeId);

            return new Dictionary<string, string>
            {
                [DefaultLanguageCode] = displayedTypeId
            };
        }
    }
}