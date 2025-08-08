using AutoFixture;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.RemotingRef.NetCore.Models;
using Fanex.RemotingRef.NetCore.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.RemotingRef.Service.Tests.Services
{
    public class SportServiceTests
    {
        private readonly IDynamicRepository _mockRepository;
        private readonly ICacheService _mockCacheService;
        private readonly SportService _sportService;
        private readonly Fixture _fixture;

        public SportServiceTests()
        {
            _mockRepository = Substitute.For<IDynamicRepository>();
            _mockCacheService = Substitute.For<ICacheService>();
            _sportService = new SportService(_mockRepository, _mockCacheService);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetSportById_WithValidId_ShouldReturnSportName()
        {
            // Arrange
            const string sportId = "1";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football"}
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal("Football", result);
        }

        [Fact]
        public void GetSportById_WithChineseLanguage_ShouldReturnChineseName()
        {
            // Arrange
            const string sportId = "1";
            const string languageCode = "zh-TW";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football", SportNameCh = "足球" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal("足球", result);
        }

        [Fact]
        public void GetSportById_WithInvalidId_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "invalid";
            const string languageCode = "en-US";

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithNonExistentId_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "999";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithEmptyCache_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "1";
            const string languageCode = "en-US";
            var sports = new List<SportModel>();

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithNullSportId_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = null;
            const string languageCode = "en-US";

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }
      
        [Theory]
        [InlineData("1", "en-US", "Football")]
        [InlineData("1", "zh-TW", "足球")]
        [InlineData("1", "ja-JP", "サッカー")]
        public void GetSportById_WithDifferentLanguages_ShouldReturnCorrectLanguage(string sportId, string languageCode, string expectedResult)
        {
            // Arrange
            var sports = new List<SportModel>
            {
                new SportModel 
                { 
                    SportType = 1,
                    SportName = "Football",
                    SportNameCh = "足球",
                    SportNameJp = "サッカー",
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSportById_WithMultipleSports_ShouldReturnCorrectOne()
        {
            // Arrange
            const string sportId = "2";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" },
                new SportModel { SportType = 2, SportName = "Basketball" },
                new SportModel { SportType = 3, SportName = "Tennis" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal("Basketball", result);
        }

        [Fact]
        public void GetSportById_ShouldCallCacheServiceWithCorrectKey()
        {
            // Arrange
            const string sportId = "1";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            _sportService.GetSportById(sportId, languageCode);

            // Assert
            _mockCacheService.Received(1).GetOrSet("Fanex.RemotingRef.NetCode.Sport", Arg.Any<Func<IEnumerable<SportModel>>>());
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("999")]
        public void GetSportById_WithNonExistentSportIds_ShouldReturnEmptyString(string sportId)
        {
            // Arrange
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" },
                new SportModel { SportType = 2, SportName = "Basketball" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithZeroId_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "0";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithNegativeId_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "-1";
            const string languageCode = "en-US";

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSportById_WithLargeNumber_ShouldReturnEmptyString()
        {
            // Arrange
            const string sportId = "999999";
            const string languageCode = "en-US";
            var sports = new List<SportModel>
            {
                new SportModel { SportType = 1, SportName = "Football" }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<SportModel>>>())
                .Returns(sports);

            // Act
            var result = _sportService.GetSportById(sportId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
} 