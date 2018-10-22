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
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var kenoBetChoiceId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetKenoBetChoice(kenoBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"KenoBetChoice?kenoBetChoiceId={kenoBetChoiceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnKenoBetChoiceGotFromService()
        {
            // Arrange.
            var kenoBetChoiceId = Fixture.Create<string>();
            var kenoBetChoiceName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(kenoBetChoiceName, null)
                });

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                out _);

            // Assert.
            Assert.Equal(kenoBetChoiceName, actualKenoBetChoiceName);
        }

        [Fact]
        public void SetKenoBetTypeIdGotFromServiceAsOutput()
        {
            // Arrange.
            var kenoBetChoiceId = Fixture.Create<string>();
            var kenoBetTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(null, kenoBetTypeId)
                });

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(kenoBetTypeId, actualKenoBetTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var kenoBetChoiceId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out _);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetChoiceName);
        }

        [Fact]
        public void SetDefaultValueAsOutputWhenServiceReturnsNull()
        {
            // Arrange.
            var kenoBetChoiceId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableKenoBetChoiceId(string unacceptableKenoBetChoiceId)
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                unacceptableKenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out _);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetChoiceName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void SetDefaultValueAsOutputForUnacceptableKenoBetChoiceId(string unacceptableKenoBetChoiceId)
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetKenoBetChoice(
                unacceptableKenoBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualKenoBetTypeId);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableKenoBetChoiceId = string.Empty;

            // Act.
            var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
                unacceptableKenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Empty(actualKenoBetChoice);
            Assert.Empty(actualKenoBetTypeId);
        }
    }
}