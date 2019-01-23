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
            var teamId = Fixture.Create<string>();
            var failedReason = Fixture.Create<string>();
            var statusCode = HttpStatusCode.InternalServerError;
            var requestRelativePath = string.Format(RequestRelativePathPattern, $"TeamName?teamId={teamId}");
            var requestUri = new Uri(RefDataTypeContainer.ServiceUri, requestRelativePath);

            Response.StatusCode = statusCode;
            Response.ReasonPhrase = failedReason;
            Response.RequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            HttpClient
                .GetAsync(Arg.Is(requestRelativePath))
                .Returns(Response);

            // Act.
            var exception = Record.Exception(() => RemotingRefService.GetTeamName(teamId, TestingLanguageCode));

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
            string resourceId = null;
            var betTypeId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"BetTypeName?betTypeId={betTypeId}");

            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));
            HttpClient
                .GetAsync(Arg.Is(expectedRequestRelativePath))
                .Returns(Response);

            // Act.
            RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguageCode,
                defaultValue,
                resourceId);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }
    }
}