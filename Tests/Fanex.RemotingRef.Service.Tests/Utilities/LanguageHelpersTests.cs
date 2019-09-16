namespace Fanex.RemotingRef.Service.Tests.Utilities
{
    using Fanex.RemotingRef.Service.Utilities;
    using Xunit;

    public class LanguageHelpersTests
    {
        [Fact]
        public void ReturnEnForUnmappedLanguageCodes()
        {
            // Arrange.
            const string unmappedLanguageCode = "fr-FR";

            // Act.
            var actualRefLanguageCode = LanguageHelpers.ToRefLanguageCode(unmappedLanguageCode);

            // Assert.
            const string expectedRefLanguageCode = "en";

            Assert.Equal(expectedRefLanguageCode, actualRefLanguageCode);
        }
    }
}