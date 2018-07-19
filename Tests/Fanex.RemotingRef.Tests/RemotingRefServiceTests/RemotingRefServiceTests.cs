namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Net;
    using System.Net.Http;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RemotingRefServiceTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void RequestTranslationReturningFailedResponseThrowsRemotingRefException()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var statusCode = Fixture.Create<HttpStatusCode>();
            var failedReason = Fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = failedReason
            };

            HttpClient.GetAsync(Arg.Any<string>()).Returns(httpResponseMessage);

            // Act.
            var exception = Record.Exception(() => RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode));

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");
            var remotingRefException = exception as RemotingRefException;

            Assert.IsType<RemotingRefException>(exception);
            Assert.Equal(expectedUrl, remotingRefException.Request);
            Assert.Equal(statusCode, remotingRefException.StatusCode);
            Assert.Equal(failedReason, remotingRefException.FailedReason);
        }

        [Fact]
        public void NullParameterIsNotIncludedInRequest()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();
            string resourceId = null;

            // Act.
            RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode, defaultValue, resourceId);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }
    }
}