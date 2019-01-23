namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _kenoBetChoiceId;

        public KenoBetChoiceTests()
        {
            _kenoBetChoiceId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"KenoBetChoice?kenoBetChoiceId={_kenoBetChoiceId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<Tuple<string, string>>()));

            // Act.
            RemotingRefService.GetKenoBetChoice(_kenoBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnKenoBetChoiceGotFromService()
        {
            // Arrange.
            var kenoBetChoiceName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(new Tuple<string, string>(kenoBetChoiceName, null)));

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
                TestingLanguageCode,
                out _);

            // Assert.
            Assert.Equal(kenoBetChoiceName, actualKenoBetChoiceName);
        }

        [Fact]
        public void SetKenoBetTypeIdGotFromServiceAsOutput()
        {
            // Arrange.
            var kenoBetTypeId = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(new Tuple<string, string>(null, kenoBetTypeId)));

            // Act.
            RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
                TestingLanguageCode,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(kenoBetTypeId, actualKenoBetTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualKenoBetChoiceName = RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
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
            var defaultValue = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            RemotingRefService.GetKenoBetChoice(
                _kenoBetChoiceId,
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