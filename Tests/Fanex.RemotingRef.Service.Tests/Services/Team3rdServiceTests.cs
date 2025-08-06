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

    public class Team3rdServiceTests : IDisposable
    {
        private readonly IDynamicRepository _mockRepository;
        private readonly ICacheService _mockCacheService;
        private readonly Team3rdService _team3rdService;
        private readonly Fixture _fixture;

        public Team3rdServiceTests()
        {
            _mockRepository = Substitute.For<IDynamicRepository>();
            _mockCacheService = Substitute.For<ICacheService>();
            _team3rdService = new Team3rdService(_mockRepository, _mockCacheService);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetTeam3rdById_WithValidId_ReturnsTeamName()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test Team";
            
            var teamModel = CreateTeam3rdModel(123, expectedName);
            var teams = new Dictionary<int, Team3rdModel> { { 123, teamModel } };
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns(teams);

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void GetTeam3rdById_WithInvalidId_ReturnsEmptyString()
        {
            // Arrange
            const string team3rdId = "invalid";
            const string languageCode = "en-US";

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetTeam3rdById_WithNonExistentId_ReturnsEmptyString()
        {
            // Arrange
            const string team3rdId = "999";
            const string languageCode = "en-US";
            
            var teams = new Dictionary<int, Team3rdModel>();
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns(teams);
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(new List<Team3rdModel>());

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetTeam3rdById_WithNullCache_LoadsFromRepository()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test Team";
            
            var teamModel = CreateTeam3rdModel(123, expectedName);
            var teams = new Dictionary<int, Team3rdModel> { { 123, teamModel } };
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns((Dictionary<int, Team3rdModel>)null);
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(new List<Team3rdModel> { teamModel });

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.Received(1).Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>());
        }

        [Fact]
        public void GetTeam3rdById_WithCacheMiss_ReloadsFromRepository()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test Team";
            
            var teamModel = CreateTeam3rdModel(123, expectedName);
            var cachedTeams = new Dictionary<int, Team3rdModel>(); // Empty cache
            var allTeams = new Dictionary<int, Team3rdModel> { { 123, teamModel } };
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns(cachedTeams);
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(new List<Team3rdModel> { teamModel });

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.Received(1).Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>());
        }

        [Fact]
        public void GetTeam3rdById_WithDifferentLanguage_ReturnsCorrectLanguage()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "ja-JP";
            const string expectedName = "テストチーム";
            
            var teamModel = CreateTeam3rdModel(123, expectedName, LanguageEnum.Jp);
            var teams = new Dictionary<int, Team3rdModel> { { 123, teamModel } };
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns(teams);

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void GetAllTeams_ReturnsCachedData()
        {
            // Arrange
            var expectedTeams = new Dictionary<int, Team3rdModel>
            {
                { 1, CreateTeam3rdModel(1, "Team 1") },
                { 2, CreateTeam3rdModel(2, "Team 2") }
            };
            
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(expectedTeams.Values.ToList());

            // Act
            var result = _team3rdService.GetAllTeams();

            // Assert
            Assert.Equal(expectedTeams.Count, result.Count);
            Assert.Equal(expectedTeams[1].TeamName, result[1].TeamName);
            Assert.Equal(expectedTeams[2].TeamName, result[2].TeamName);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData(null)]
        public void GetTeam3rdById_WithInvalidInputs_ReturnsEmptyString(string invalidId)
        {
            // Arrange
            const string languageCode = "en-US";

            // Act
            var result = _team3rdService.GetTeam3rdById(invalidId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetTeam3rdById_WithEmptyCacheAndRepository_ReturnsEmptyString()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "en-US";
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns((Dictionary<int, Team3rdModel>)null);
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(new List<Team3rdModel>());

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAllTeams_WithEmptyRepository_ReturnsEmptyDictionary()
        {
            // Arrange
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(new List<Team3rdModel>());

            // Act
            var result = _team3rdService.GetAllTeams();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetTeam3rdById_WithCacheHit_DoesNotCallRepository()
        {
            // Arrange
            const string team3rdId = "123";
            const string languageCode = "en-US";
            const string expectedName = "Test Team";
            
            var teamModel = CreateTeam3rdModel(123, expectedName);
            var teams = new Dictionary<int, Team3rdModel> { { 123, teamModel } };
            
            _mockCacheService.Get<Dictionary<int, Team3rdModel>>(Arg.Any<string>()).Returns(teams);

            // Act
            var result = _team3rdService.GetTeam3rdById(team3rdId, languageCode);

            // Assert
            Assert.Equal(expectedName, result);
            _mockRepository.DidNotReceive().Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>());
        }
        
        [Fact]
        public void GetAllTeams_CallsRepositoryWithCorrectCriteria()
        {
            // Arrange
            var teams = new List<Team3rdModel> { CreateTeam3rdModel(1, "Team 1") };
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(teams);

            // Act
            _team3rdService.GetAllTeams();

            // Assert
            _mockRepository.Received(1).Fetch<Team3rdModel>(Arg.Is<GetTeam3rdCriteria>(criteria => criteria != null));
        }

        [Fact]
        public void GetAllTeams_SetsCacheWithCorrectKey()
        {
            // Arrange
            var teams = new List<Team3rdModel> { CreateTeam3rdModel(1, "Team 1") };
            _mockRepository.Fetch<Team3rdModel>(Arg.Any<GetTeam3rdCriteria>()).Returns(teams);

            // Act
            _team3rdService.GetAllTeams();

            // Assert
            _mockCacheService.Received(1).Set(
                Arg.Is<string>(key => key == "Fanex.RemotingRef.NetCode.Team3rd"),
                Arg.Any<Dictionary<int, Team3rdModel>>(),
                Arg.Any<CacheItemOptions>());
        }

        private Team3rdModel CreateTeam3rdModel(int id, string name, LanguageEnum language = LanguageEnum.En)
        {
            var model = _fixture.Create<Team3rdModel>();
            model.Team3rdId = id;
            
            // Set the name for the specified language
            switch (language)
            {
                case LanguageEnum.En:
                    model.TeamName = name;
                    break;
                case LanguageEnum.Jp:
                    model.TeamNameJp = name;
                    break;
                case LanguageEnum.Ko:
                    model.TeamNameKo = name;
                    break;
                case LanguageEnum.Th:
                    model.TeamNameTh = name;
                    break;
                case LanguageEnum.Vn:
                    model.TeamNameVn = name;
                    break;
                case LanguageEnum.Ch:
                    model.TeamNameSc = name;
                    break;
                case LanguageEnum.Cn:
                    model.TeamNameTc = name;
                    break;
                default:
                    model.TeamName = name;
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