namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RemotingRefServiceTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void RequestTranslationReturningFailedResponseThrowsRemotingRefException()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var statusCode = Fixture.Create<HttpStatusCode>();
            var failedReason = Fixture.Create<string>();
            var requestRelativePath = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");
            var requestUri = new Uri(RefDataTypeContainer.ServiceUri, requestRelativePath);
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = failedReason,
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri)
            };

            HttpClient.GetAsync(Arg.Any<string>()).Returns(httpResponseMessage);

            // Act.
            var exception = Record.Exception(() => RemotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode));

            // Assert.
            var remotingRefException = exception as RemotingRefException;

            Assert.IsType<RemotingRefException>(exception);
            Assert.Equal(requestUri, remotingRefException.RequestUri);
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