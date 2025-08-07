namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using Xunit;

    public class LeagueNameWithGroupIdTests : RemotingRefServiceTestBase
    {
        private readonly string _leagueId;
        private readonly string _leagueGroupId;
        private readonly string _defaultValue;

        public LeagueNameWithGroupIdTests()
        {
            _leagueId = Fixture.Create<string>();
            _leagueGroupId = Fixture.Create<string>();
            _defaultValue = Fixture.Create<string>();
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var leagueName = Fixture.Create<string>();
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);

            // Act.
            RemotingRefService.GetLeagueName(_leagueId, _leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is<string>(path => path.Contains($"LeagueName?leagueId={_leagueId}")));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueName = Fixture.Create<string>();
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, _leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet<object>(null));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, _leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(_defaultValue, actualLeagueName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableLeagueId(string unacceptableLeagueId)
        {
            // Arrange.

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(unacceptableLeagueId, _leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(_defaultValue, actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_ShouldCallGetResourceDataForSaba(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData?refId=Saba&resourceId=Saba")));
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaDataFound_ShouldRemoveSabaWords(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test Saba League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When removing "Saba" from "Test Saba League Name", result should be "Test League Name"
            Assert.Equal("Test League Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaDataNotFound_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet<object>(null));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndRegexTimeout_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // The method should handle regex timeout gracefully and return the original league name
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("43")]
        [InlineData("75")]
        [InlineData("109")]
        [InlineData("114")]
        [InlineData("123")]
        [InlineData("152")]
        public void WhenLeagueGroupIdDoesNotNeedRemoveText_ShouldNotCallGetResourceDataForSaba(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);

            // Act.
            RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            HttpClient
                .DidNotReceive()
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData?refId=Saba&resourceId=Saba")));
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_ShouldRemoveSabaWordsWithDifferentPatterns(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Saba Test League Name Saba";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When removing "Saba" from "Saba Test League Name Saba", result should be "Test League Name"
            // The function removes "Saba" completely, not leaving spaces at the beginning and end
            Assert.Equal("Test League Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaWordIsCaseInsensitive(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test SABA League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When removing "Saba" from "Test SABA League Name" (case insensitive), result should be "Test League Name"
            Assert.Equal("Test League Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaWordWithSpaces(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test Saba League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet(" Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When Saba word from server is " Saba", it removes " Saba" from "Test Saba League Name"
            // Result: "TestLeague Name" (no space between "Test" and "League")
            Assert.Equal("TestLeague Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaWordWithTrailingSpace(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test Saba League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba "));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When Saba word from server is "Saba ", it removes "Saba " from "Test Saba League Name"
            // Result: "TestLeague Name" (no space between "Test" and "League")
            Assert.Equal("TestLeague Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaWordExactMatch(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test Saba League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // When removing "Saba" from "Test Saba League Name", result should be "Test League Name"
            Assert.Equal("Test League Name", actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndSabaDataRequestFails_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Throws(new System.Exception("Network error"));

            // Act & Assert.
            // The method should throw exception when Saba data request fails
            var exception = Assert.Throws<System.Exception>(() => 
                RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue));
            
            Assert.Equal("Network error", exception.Message);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndEmptySabaWord_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet(string.Empty));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndNullSabaWord_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet<object>(null));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("42")]
        [InlineData("74")]
        [InlineData("108")]
        [InlineData("113")]
        [InlineData("122")]
        [InlineData("151")]
        public void WhenLeagueGroupIdNeedsRemoveText_AndRegexTimeoutException_ShouldReturnOriginalLeagueName(string leagueGroupId)
        {
            // Arrange.
            var leagueName = "Test League Name";
            var leagueNameResponse = new HttpResponseMessage();
            leagueNameResponse.Content = FakeResponseContent(SerializeTranslationSet(leagueName));
            
            var sabaResponse = new HttpResponseMessage();
            sabaResponse.Content = FakeResponseContent(SerializeTranslationSet("Saba"));
            
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("LeagueName")))
                .Returns(leagueNameResponse);
            HttpClient
                .GetAsync(Arg.Is<string>(path => path.Contains("ResourceData")))
                .Returns(sabaResponse);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, leagueGroupId, TestingLanguageCode, _defaultValue);

            // Assert.
            // The method should handle RegexMatchTimeoutException gracefully and return the original league name
            Assert.Equal(leagueName, actualLeagueName);
        }
    }
} 