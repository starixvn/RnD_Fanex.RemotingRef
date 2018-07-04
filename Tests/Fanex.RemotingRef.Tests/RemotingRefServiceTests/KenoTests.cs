namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetKenoBetChoiceSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var kenoBetChoiceId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetKenoBetChoice(kenoBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"KenoBetChoice?kenoBetChoiceId={kenoBetChoiceId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetKenoBetChoiceReturnsValueGotFromService()
        {
            // Arrange.
            var kenoBetChoiceId = _fixture.Create<string>();
            var kenoBetChoiceName = _fixture.Create<string>();
            var kenoBetTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>> { { TestingLanguageCode, new Tuple<string, string>(kenoBetChoiceName, kenoBetTypeId) } });

            // Act.
            var actualKenoBetChoiceName = _remotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(kenoBetChoiceName, actualKenoBetChoiceName);
            Assert.Equal(kenoBetTypeId, actualKenoBetTypeId);
        }

        [Fact]
        public void GetKenoBetChoiceWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var kenoBetChoiceId = _fixture.Create<string>();
            var defaultValue = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetChoiceName = _remotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetChoiceName);
            Assert.Equal(defaultValue, actualKenoBetTypeId);
        }

        [Fact]
        public void GetKenoBetChoiceWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var kenoBetChoiceId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = _remotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Empty(actualTeamName);
            Assert.Empty(actualKenoBetTypeId);
        }

        [Fact]
        public void GetKenoBetTypeSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var kenoBetTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"KenoBetType?kenoBetTypeId={kenoBetTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetKenoBetTypeReturnsValueGotFromService()
        {
            // Arrange.
            var kenoBetTypeId = _fixture.Create<string>();
            var kenoBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, kenoBetTypeName } });

            // Act.
            var actualKenoBetTypeName = _remotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(kenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void GetKenoBetTypeWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var kenoBetTypeId = _fixture.Create<string>();
            var defaultKenoBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = _remotingRefService.GetKenoBetType(
                kenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void GetKenoBetTypeWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var kenoBetTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetTypeName = _remotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }
    }
}