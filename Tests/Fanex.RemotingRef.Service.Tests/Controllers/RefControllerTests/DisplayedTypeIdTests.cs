namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RefControllerTestBase
    {
        private readonly string _betTypeId;

        public DisplayedTypeIdTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetDisplayedTypeIdOfRefDataHelper()
        {
            // Act.
            RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            RefData
                .Received()
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerBetTypeId()
        {
            // Act.
            RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            RefData
                .Received(1)
                .GetDisplayedTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            RefData
                .Received()
                .GetDisplayedTypeId(Arg.Is(_betTypeId));
        }

        [Fact]
        public void ResultTakesDefaultLanguageCodeAsKey()
        {
            // Arrange.
            FakeDisplayedTypeId();

            // Act.
            var actualResult = RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(new[] { DefaultLanguageCode }, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedDisplayedTypeIds = FakeDisplayedTypeId();

            // Act.
            var actualResult = RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(expectedDisplayedTypeIds, actualResult.Values);
        }

        private List<string> FakeDisplayedTypeId()
        {
            var displayedTypeId = Fixture.Create<string>();

            RefData
                .GetDisplayedTypeId(Arg.Is(_betTypeId))
                .Returns(displayedTypeId);

            return new List<string>
            {
                displayedTypeId
            };
        }

        [Fact]
        public void ReturnEmptylIfResultIsNullValue()
        {
            // Arrange.
            RefData
                .GetDisplayedTypeId(Arg.Is(_betTypeId))
                .Returns((string)null);

            // Act.
            var actualResult = RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultIsEmptyString()
        {
            // Arrange.
            RefData
                .GetDisplayedTypeId(Arg.Is(_betTypeId))
                .Returns(string.Empty);

            // Act.
            var actualResult = RefController.DisplayedTypeId(_betTypeId);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }
    }
}