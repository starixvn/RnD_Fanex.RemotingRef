namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _kenoBetChoiceId;

        public KenoBetChoiceTests()
            => _kenoBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetKenoBetChoice(_kenoBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"KenoBetChoice?kenoBetChoiceId={_kenoBetChoiceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var kenoBetChoiceName = Fixture.Create<string>();
            var kenoBetTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(kenoBetChoiceName, kenoBetTypeId)
                });

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(kenoBetChoiceName, actualKenoBetChoiceName);
            Assert.Equal(kenoBetTypeId, actualKenoBetTypeId);
        }

        [Fact]
        public void SetProvidedDefaultValueAsOutputAndReturnIfServiceReturnsNull()
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetChoiceName);
            Assert.Equal(defaultValue, actualKenoBetTypeId);
        }

        [Fact]
        public void SetEmptyStringAsOutputAndReturnIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Empty(actualTeamName);
            Assert.Empty(actualKenoBetTypeId);
        }
    }
}