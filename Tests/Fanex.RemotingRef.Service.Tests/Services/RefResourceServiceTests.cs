using AutoFixture;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.RemotingRef.NetCore.Models;
using Fanex.RemotingRef.NetCore.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.RemotingRef.Service.Tests.Services
{
    public class RefResourceServiceTests
    {
        private readonly IDynamicRepository _mockRepository;
        private readonly ICacheService _mockCacheService;
        private readonly RefResourceService _refResourceService;
        private readonly Fixture _fixture;

        public RefResourceServiceTests()
        {
            _mockRepository = Substitute.For<IDynamicRepository>();
            _mockCacheService = Substitute.For<ICacheService>();
            _refResourceService = new RefResourceService(_mockRepository, _mockCacheService);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetRefResourceById_WithValidRefIdAndResourceId_ShouldReturnResourceName()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)"
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal("(Via Lucky)", result);
        }

        [Fact]
        public void GetRefResourceById_WithChineseLanguage_ShouldReturnChineseName()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "LuckyBet";
            const string languageCode = "zh-TW";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)",
                    ResourceCh = "(通过幸运)" // Assuming this is the Chinese translation
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal("(通过幸运)", result);
        }

        [Fact]
        public void GetRefResourceById_WithNonExistentRefId_ShouldReturnEmptyString()
        {
            // Arrange
            const string refId = "NonExistent";
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetRefResourceById_WithNonExistentResourceId_ShouldReturnEmptyString()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "NonExistent";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetRefResourceById_WithEmptyCache_ShouldReturnEmptyString()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>();

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetRefResourceById_WithNullRefId_ShouldReturnEmptyString()
        {
            // Arrange
            const string refId = null;
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetRefResourceById_WithNullResourceId_ShouldReturnEmptyString()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = null;
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(string.Empty, result);
        }
        
        [Theory]
        [InlineData("ComboParlay", "LuckyBet", "en-US", "(Via Lucky)")]
        [InlineData("ComboParlay", "LuckyBet", "zh-TW", "(通过幸运)")]
        [InlineData("ComboParlay", "LuckyBet", "ja-JP", "(ラッキー経由)")]
        public void GetRefResourceById_WithDifferentLanguages_ShouldReturnCorrectLanguage(string refId, string resourceId, string languageCode, string expectedResult)
        {
            // Arrange
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)",
                    ResourceCh = "(通过幸运)", // Assuming this is the Chinese translation
                    ResourceJp = "(ラッキー経由)" // Assuming this is the Japanese translation
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetRefResourceById_WithMultipleResources_ShouldReturnCorrectOne()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet",
                    ResourceName = "(Via Lucky)" 
                },
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "OtherBet",
                    ResourceName = "Other Resource" 
                },
                new RefResourceModel 
                { 
                    RefId = "OtherRef", 
                    ResourceId = "LuckyBet",
                    ResourceName = "Other Ref Resource" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            var result = _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            Assert.Equal("(Via Lucky)", result);
        }

        [Fact]
        public void GetRefResourceById_ShouldCallCacheServiceWithCorrectKey()
        {
            // Arrange
            const string refId = "ComboParlay";
            const string resourceId = "LuckyBet";
            const string languageCode = "en-US";
            var refResources = new List<RefResourceModel>
            {
                new RefResourceModel 
                { 
                    RefId = "ComboParlay", 
                    ResourceId = "LuckyBet", 
                    ResourceName = "(Via Lucky)" 
                }
            };

            _mockCacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<RefResourceModel>>>())
                .Returns(refResources);

            // Act
            _refResourceService.GetRefResourceById(refId, resourceId, languageCode);

            // Assert
            _mockCacheService.Received(1).GetOrSet("Fanex.RemotingRef.NetCode.RefResource", Arg.Any<Func<IEnumerable<RefResourceModel>>>());
        }
    }
} 