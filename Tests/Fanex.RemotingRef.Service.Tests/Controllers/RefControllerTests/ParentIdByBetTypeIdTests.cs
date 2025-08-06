namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RefControllerTestBase
    {
        private readonly string _betTypeId;

        public ParentIdByBetTypeIdTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetParentIdByBetTypeIdOfRefDataHelper()
        {
            // Act.
            RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            RefData
                .Received()
                .GetParentIdByBetTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerBetTypeId()
        {
            // Act.
            RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            RefData
                .Received(1)
                .GetParentIdByBetTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Arrange.
            FakeParentIdByBetTypeId();

            // Act.
            var actualParentIdByBetTypeId = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.Equal(new[] { DefaultLanguageCode }, actualParentIdByBetTypeId.Keys);
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeParentIdByBetTypeId();

            // Act.
            var actualResult = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.Equal(new[] { DefaultLanguageCode }, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedParentIdByBetTypeId = FakeParentIdByBetTypeId();

            // Act.
            var actualResult = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.Equal(expectedParentIdByBetTypeId, actualResult.Values);
        }

        private List<string> FakeParentIdByBetTypeId()
        {
            var parentBetTypeId = Fixture.Create<string>();

            RefData
                .GetParentIdByBetTypeId(Arg.Is(_betTypeId))
                .Returns(parentBetTypeId);

            return new List<string>
            {
                parentBetTypeId
            };
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            RefData
                .GetParentIdByBetTypeId(Arg.Is(_betTypeId))
                .Returns((string)null);

            // Act.
            var actualResult = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            RefData
                .GetParentIdByBetTypeId(Arg.Is(_betTypeId))
                .Returns(string.Empty);

            // Act.
            var actualResult = RefController.ParentIdByBetTypeId(_betTypeId);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }
    }
}