namespace Fanex.RemotingRef.Service.Tests.Services
{
    using Xunit;

    public class Team3rdServiceDiscoveryTest
    {
        [Fact]
        public void Team3rdService_SimpleTest_ShouldPass()
        {
            // Arrange
            const int expected = 2;

            // Act
            var actual = 1 + 1;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("123", "en-US")]
        [InlineData("456", "ja-JP")]
        [InlineData("789", "ko-KR")]
        public void Team3rdService_TheoryTest_ShouldPass(string teamId, string language)
        {
            // Arrange
            var expectedTeamId = teamId;
            var expectedLanguage = language;

            // Act
            var actualTeamId = teamId;
            var actualLanguage = language;

            // Assert
            Assert.Equal(expectedTeamId, actualTeamId);
            Assert.Equal(expectedLanguage, actualLanguage);
        }
    }
} 