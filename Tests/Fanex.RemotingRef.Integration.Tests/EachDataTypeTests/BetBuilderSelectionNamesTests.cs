namespace Fanex.RemotingRef.Integration.Tests.EachDataTypeTests
{
    using System;
    using System.ComponentModel;
    using Xunit;

    public class BetBuilderSelectionNamesTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: all selectionIds are existing | GET: one selection name associates with one selectionId")]
        public void GetSelectionNameForEachExistingSelectionId()
        {
            // Arrange.
            var existingSelectionIds = new[] { 60, 61 };

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            Assert.Equal(existingSelectionIds.Length, actualSelectionNames.Length);
        }

        [Fact]
        [Description("GIVE: all selectionIds are existing | GET: correct selection name of each selectionId")]
        public void GetCorrectSelectionNameOfEachExistingSelectionId()
        {
            // Arrange.
            var existingSelectionIds = new[] { 62, 63 };

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            var expectedSelectionNames = new[]
            {
                "Result - Manchester City win",
                "Player cards - Fabian Delph carded"
            };

            Assert.Equal(expectedSelectionNames, actualSelectionNames);
        }

        [Fact]
        [Description("GIVE: some selectionIds are unexisting | GET: only existing selection names")]
        public void ExcludeUnexistingSelectionInTheResult()
        {
            // Arrange.
            var existingSelectionIds = new[] { 64, -1, 65 };

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            var expectedSelectionNames = new[]
            {
                "Player goals - Sergio Agüero score anytime",
                "Match corners - over 7.5"
            };

            Assert.Equal(expectedSelectionNames, actualSelectionNames);
        }

        [Fact]
        [Description("GIVE: all selectionIds are unexisting | GET: empty array")]
        public void GetEmptyArrayForAllUnexistingSelectionIds()
        {
            // Arrange.
            var existingSelectionIds = new[] { -2, -3 };

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            Assert.Empty(actualSelectionNames);
        }

        [Fact]
        [Description("GIVE: passed selectionIds array is null | GET: null")]
        public void GetNullForNullArrayOfSelectionIds()
        {
            // Arrange.
            int[] existingSelectionIds = null;

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            Assert.Null(actualSelectionNames);
        }

        [Fact]
        [Description("GIVE: passed selectionIds array is empty | GET: null")]
        public void GetNullForEmptyArrayOfSelectionIds()
        {
            // Arrange.
            var existingSelectionIds = Array.Empty<int>();

            // Act.
            var actualSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(existingSelectionIds, TestingLanguage);

            // Assert.
            Assert.Null(actualSelectionNames);
        }
    }
}