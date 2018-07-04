namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using AutoFixture;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Fanex.RemotingRef.Utilities.WebClient;
    using NSubstitute;
    using Xunit;

    [Collection("Run with Single RefDataTypeContainer due to conflicting set up RefDataTypeContainer")]
    public partial class RemotingRefServiceTests
    {
        private const string ApplicationName = "UnitTest_AppName";
        private const string RequestUrlPattern = "api/ref/{0}&languages=en-US&applicationName=UnitTest_AppName";
        private const string TestingLanguageCode = RefDataTypeHandlerOptions.DefaultLanguage;

        private readonly IHttpHandler _httpClient;
        private readonly ISerializer _serializer;
        private readonly IRemotingRefService _remotingRefService;
        private readonly Fixture _fixture;

        public RemotingRefServiceTests()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri("http://localhost:10000"))
                .FromApplication(ApplicationName)
                .SupportOnlyDefaultLanguage();

            _serializer = Substitute.For<ISerializer>();

            _httpClient = Substitute.For<IHttpHandler>();
            _httpClient.GetAsync(Arg.Any<string>()).Returns(FakeHttpResponseMessage());

            _remotingRefService = new RemotingRefService(_httpClient, _serializer);

            _fixture = new Fixture();
        }

        private static HttpResponseMessage FakeHttpResponseMessage()
        {
            var httpResponseMessage = Substitute.For<HttpResponseMessage>();
            httpResponseMessage.Content = new StreamContent(new MemoryStream(new byte[] { }));
            return httpResponseMessage;
        }

        [Fact]
        public void RequestTranslationReturningFailedResponseThrowsRemotingRefException()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var statusCode = _fixture.Create<HttpStatusCode>();
            var failedReason = _fixture.Create<string>();
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = failedReason
            };

            _httpClient.GetAsync(Arg.Any<string>()).Returns(httpResponseMessage);

            // Act.
            var exception = Record.Exception(() => _remotingRefService.GetBetTypeName(betTypeId, TestingLanguageCode));

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"BetTypeName?betTypeId={betTypeId}");
            var remotingRefException = exception as RemotingRefException;

            Assert.IsType<RemotingRefException>(exception);
            Assert.Equal(expectedUrl, remotingRefException.Request);
            Assert.Equal(statusCode, remotingRefException.StatusCode);
            Assert.Equal(failedReason, remotingRefException.FailedReason);
        }
    }
}