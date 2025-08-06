namespace Fanex.RemotingRef.Service.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using NSubstitute;
    using PublicEnum.Language;
    using Xunit;

    public class League3rdServiceTests : IDisposable
    {
        private readonly IDynamicRepository _mockRepository;
        private readonly ICacheService _mockCacheService;
        private readonly League3rdService _league3rdService;
        private readonly Fixture _fixture;

        public League3rdServiceTests()
        {
            _mockRepository = Substitute.For<IDynamicRepository>();
            _mockCacheService = Substitute.For<ICacheService>();
            _league3rdService = new League3rdService(_mockRepository, _mockCacheService);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetLeague3rdById_WithValidId_ReturnsLeagueName()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test League";

            var leagueModel = CreateLeague3rdModel(123, expectedName);
            var leagues = new Dictionary<int, League3rdModel> { { 123, leagueModel } };

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns(leagues);

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void GetLeague3rdById_WithInvalidId_ReturnsEmptyString()
        {
            // Arrange
            const string league3rdId = "invalid";
            const string languageCode = "en-US";

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetLeague3rdById_WithNonExistentId_ReturnsEmptyString()
        {
            // Arrange
            const string league3rdId = "999";
            const string languageCode = "en-US";

            var leagues = new Dictionary<int, League3rdModel>();
            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns(leagues);
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(new List<League3rdModel>());

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetLeague3rdById_WithNullCache_LoadsFromRepository()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test League";

            var leagueModel = CreateLeague3rdModel(123, expectedName);
            var leagues = new Dictionary<int, League3rdModel> { { 123, leagueModel } };

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns((Dictionary<int, League3rdModel>)null);
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(new List<League3rdModel> { leagueModel });

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.Received(1).Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>());
        }

        [Fact]
        public void GetLeague3rdById_WithCacheMiss_ReloadsFromRepository()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test League";

            var leagueModel = CreateLeague3rdModel(123, expectedName);
            var cachedLeagues = new Dictionary<int, League3rdModel>(); // Empty cache
            var allLeagues = new Dictionary<int, League3rdModel> { { 123, leagueModel } };

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns(cachedLeagues);
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(new List<League3rdModel> { leagueModel });

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.Received(1).Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>());
        }

        [Fact]
        public void GetLeague3rdById_WithDifferentLanguage_ReturnsCorrectLanguage()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "ja-JP";
            const string expectedName = "テストリーグ";

            var leagueModel = CreateLeague3rdModel(123, expectedName, LanguageEnum.Jp);
            var leagues = new Dictionary<int, League3rdModel> { { 123, leagueModel } };

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns(leagues);

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void GetAllLeagues_ReturnsCachedData()
        {
            // Arrange
            var expectedLeagues = new Dictionary<int, League3rdModel>
            {
                { 1, CreateLeague3rdModel(1, "League 1") },
                { 2, CreateLeague3rdModel(2, "League 2") }
            };

            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(expectedLeagues.Values.ToList());

            // Act
            var result = _league3rdService.GetAllLeagues();

            // Assert
            Assert.Equal(expectedLeagues.Count, result.Count);
            Assert.Equal(expectedLeagues[1].LeagueName, result[1].LeagueName);
            Assert.Equal(expectedLeagues[2].LeagueName, result[2].LeagueName);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData(null)]
        public void GetLeague3rdById_WithInvalidInputs_ReturnsEmptyString(string invalidId)
        {
            // Arrange
            const string languageCode = "en-US";

            // Act
            var result = _league3rdService.GetLeague3rdById(invalidId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetLeague3rdById_WithEmptyCacheAndRepository_ReturnsEmptyString()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "en-US";

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns((Dictionary<int, League3rdModel>)null);
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(new List<League3rdModel>());

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAllLeagues_WithEmptyRepository_ReturnsEmptyDictionary()
        {
            // Arrange
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(new List<League3rdModel>());

            // Act
            var result = _league3rdService.GetAllLeagues();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetLeague3rdById_WithCacheHit_DoesNotCallRepository()
        {
            // Arrange
            const string league3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test League";

            var leagueModel = CreateLeague3rdModel(123, expectedName);
            var leagues = new Dictionary<int, League3rdModel> { { 123, leagueModel } };

            _mockCacheService.Get<Dictionary<int, League3rdModel>>(Arg.Any<string>()).Returns(leagues);

            // Act
            var result = _league3rdService.GetLeague3rdById(league3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.DidNotReceive().Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>());
        }

        [Fact]
        public void GetAllLeagues_CallsRepositoryWithCorrectCriteria()
        {
            // Arrange
            var leagues = new List<League3rdModel> { CreateLeague3rdModel(1, "League 1") };
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(leagues);

            // Act
            _league3rdService.GetAllLeagues();

            // Assert
            _mockRepository.Received(1).Fetch<League3rdModel>(Arg.Is<GetLeague3rdCriteria>(criteria => criteria != null));
        }

        [Fact]
        public void GetAllLeagues_SetsCacheWithCorrectKey()
        {
            // Arrange
            var leagues = new List<League3rdModel> { CreateLeague3rdModel(1, "League 1") };
            _mockRepository.Fetch<League3rdModel>(Arg.Any<GetLeague3rdCriteria>()).Returns(leagues);

            // Act
            _league3rdService.GetAllLeagues();

            // Assert
            _mockCacheService.Received(1).Set(
                Arg.Is<string>(key => key == "Fanex.RemotingRef.NetCode.League3rd"),
                Arg.Any<Dictionary<int, League3rdModel>>(),
                Arg.Any<CacheItemOptions>());
        }

        private League3rdModel CreateLeague3rdModel(int id, string name, LanguageEnum language = LanguageEnum.En)
        {
            var model = _fixture.Create<League3rdModel>();
            model.League3rdId = id;

            // Set the name for the specified language
            switch (language)
            {
                case LanguageEnum.En:
                    model.LeagueName = name;
                    break;
                case LanguageEnum.Jp:
                    model.LeagueNameJp = name;
                    break;
                case LanguageEnum.Ko:
                    model.LeagueNameKo = name;
                    break;
                case LanguageEnum.Th:
                    model.LeagueNameTh = name;
                    break;
                case LanguageEnum.Vn:
                    model.LeagueNameVn = name;
                    break;
                case LanguageEnum.Ch:
                    model.LeagueNameSc = name;
                    break;
                case LanguageEnum.Cn:
                    model.LeagueNameTc = name;
                    break;
                default:
                    model.LeagueName = name;
                    break;
            }

            return model;
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}