using AutoFixture;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.RemotingRef.NetCore.Models;
using Fanex.RemotingRef.NetCore.Services;
using Fanex.RemotingRef.NetCore.Services.Interface;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.RemotingRef.Service.Tests.Services
{
    public class BettypeServiceTests
    {
        private readonly IDynamicRepository _mockRepository;
        private readonly ICacheService _mockCacheService;
        private readonly IRefResourceService _mockRefResourceService;
        private readonly BettypeService _bettypeService;
        private readonly Fixture _fixture;

        public BettypeServiceTests()
        {
            _mockRepository = Substitute.For<IDynamicRepository>();
            _mockCacheService = Substitute.For<ICacheService>();
            _mockRefResourceService = Substitute.For<IRefResourceService>();
            _bettypeService = new BettypeService(_mockRepository, _mockCacheService, _mockRefResourceService);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetBettypeById_WithValidId_ShouldReturnBetTypeName()
        {
            // Arrange
            const string bettypeId = "1";
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap"}
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, languageCode);

            // Assert
            Assert.Equal("Handicap", result);
        }

        [Fact]
        public void GetBettypeById_WithInvalidId_ShouldReturnEmptyString()
        {
            // Arrange
            const string bettypeId = "invalid";
            const string languageCode = "en-US";

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetBettypeById_WithNonExistentId_ShouldReturnEmptyString()
        {
            // Arrange
            const string bettypeId = "999";
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetBettypeById_WithResourceIdAndRefNo_ShouldReturnBetTypeNameWithLuckyVia()
        {
            // Arrange
            const string bettypeId = "1";
            const string resourceId = "01";
            const long refNo = 100_000_000_001;
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);
            _mockRefResourceService.GetRefResourceById("ComboParlay", "LuckyBet", languageCode)
                .Returns("(Via Lucky)");

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, refNo, languageCode);

            // Assert
            Assert.Equal("Handicap (Via Lucky)", result);
        }

        [Fact]
        public void GetBettypeById_WithRefNoUnderThreshold_ShouldNotAddLuckyVia()
        {
            // Arrange
            const string bettypeId = "1";
            const string resourceId = "01";
            const long refNo = 99_000_000_000;
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, refNo, languageCode);

            // Assert
            Assert.Equal("Handicap", result);
            _mockRefResourceService.DidNotReceive().GetRefResourceById(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void GetBettypeById_WithBaseballSpecialBetType_ShouldHandleBaseballLogic()
        {
            // Arrange
            const string bettypeId = "8810";
            const string resourceId = "01";
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 8810, TypeName = "X Race To 1 Points" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, null, languageCode);

            // Assert
            Assert.Equal("First Race To 1 Points", result);
        }

        [Fact]
        public void GetBettypeById_WithBaseballSpecialBetTypeAndNonDefaultLanguage_ShouldUseNormalLogic()
        {
            // Arrange
            const string bettypeId = "8810";
            const string resourceId = "01";
            const string languageCode = "zh-TW";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 8810, TypeName = "X Race To 1 Points" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, null, languageCode);

            // Assert
            Assert.Equal("1 Race To 1 Points", result);
        }

        [Theory]
        [InlineData("8810", "1", "First")]
        [InlineData("8811", "2", "2nd")]
        [InlineData("8812", "3", "3rd")]
        [InlineData("8813", "4", "4th")]
        [InlineData("8823", "5", "5th")]
        [InlineData("8824", "6", "6th")]
        [InlineData("8824", "7", "7th")]
        [InlineData("8824", "8", "8th")]
        [InlineData("8824", "9", "9th")]
        public void GetBettypeById_WithBaseballSpecialBetTypes_ShouldReplaceCorrectly(string bettypeId, string resourceId, string expectedReplacement)
        {
            // Arrange
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = int.Parse(bettypeId), TypeName = "X Race To 1 Points"}
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, null, languageCode);

            // Assert
            Assert.Equal($"{expectedReplacement} Race To 1 Points", result);
        }

        [Fact]
        public void GetBettypeById_WithBaseballSpecialBetTypeAndLargeResourceId_ShouldKeepOriginalResourceId()
        {
            // Arrange
            const string bettypeId = "8810";
            const string resourceId = "99";
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 8810, TypeName = "X Race To 1 Points" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, null, languageCode);

            // Assert
            Assert.Equal("99 Race To 1 Points", result);
        }

        [Fact]
        public void GetBettypeById_WithNullRefNo_ShouldNotAddLuckyVia()
        {
            // Arrange
            const string bettypeId = "1";
            const string resourceId = "01";
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, null, languageCode);

            // Assert
            Assert.Equal("Handicap", result);
            _mockRefResourceService.DidNotReceive().GetRefResourceById(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void GetBettypeById_WithEmptyLuckyVia_ShouldReturnBetTypeNameOnly()
        {
            // Arrange
            const string bettypeId = "1";
            const string resourceId = "01";
            const long refNo = 100_000_000_001;
            const string languageCode = "en-US";
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { TypeId = 1, TypeName = "Handicap" }
            };

            _mockCacheService.GetOrSet(Arg.Is<string>(s => s == "Fanex.RemotingRef.NetCode.Bettype"), Arg.Any<Func<IEnumerable<BetTypeModel>>>())
                .Returns(betTypes);
            _mockRefResourceService.GetRefResourceById("ComboParlay", "LuckyBet", languageCode)
                .Returns(string.Empty);

            // Act
            var result = _bettypeService.GetBettypeById(bettypeId, resourceId, refNo, languageCode);

            // Assert
            Assert.Equal("Handicap", result);
        }
    }
} 