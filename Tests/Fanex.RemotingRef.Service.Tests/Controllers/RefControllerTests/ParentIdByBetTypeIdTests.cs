namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RefControllerBaseTests
    {
        private readonly string _betTypeId;

        public ParentIdByBetTypeIdTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void CallGetParentIdByBetTypeIdOfRefDataHelperOnlyOneTime()
        {
            // Act.
            RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            RefData.Received(1).GetParentIdByBetTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void ReturnResultWithDefaultLanguageCodesAsKey()
        {
            // Arrange.
            FakeParentIdByBetTypeId();

            // Act.
            var actualParentIdByBetTypeId = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.Equal(new[] { DefaultLanguageCode }, actualParentIdByBetTypeId.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetParentIdByBetTypeIdOfRefDataHelper()
        {
            // Arrange.
            var expectedParentIdByBetTypeId = FakeParentIdByBetTypeId();

            // Act.
            var actualParentIdByBetTypeId = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.Equal(expectedParentIdByBetTypeId, actualParentIdByBetTypeId);
        }

        private Dictionary<string, string> FakeParentIdByBetTypeId()
        {
            var parentBetTypeId = Fixture.Create<string>();

            RefData.GetParentIdByBetTypeId(Arg.Is(_betTypeId)).Returns(parentBetTypeId);

            return new Dictionary<string, string>
            {
                [DefaultLanguageCode] = parentBetTypeId
            };
        }
    }
}