namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.Json;
    using AutoFixture;
    using Xunit;

    public class RemotingRefExceptionTests
    {
        private readonly Uri _requestUri;
        private readonly HttpStatusCode _statusCode;
        private readonly string _failedReason;
        private readonly RemotingRefException _exception;

        public RemotingRefExceptionTests()
        {
            var fixture = new Fixture();

            _requestUri = fixture.Create<Uri>();
            _statusCode = fixture.Create<HttpStatusCode>();
            _failedReason = fixture.Create<string>();
            _exception = new RemotingRefException(_requestUri, _statusCode, _failedReason);
        }

        [Fact]
        public void SerializeExceptionAlwaysDoesNotThrowException()
        {
            // Act.
            var unexpectedException = Record.Exception(() => SerializeException(_exception));

            // Assert.
            Assert.Null(unexpectedException);
        }

        [Fact]
        public void SerializeExceptionReturnsArrayOfBytes()
        {
            // Act.
            var actualResult = SerializeException(_exception);

            // Assert.
            Assert.NotEmpty(actualResult);
        }

        [Fact]
        public void SerializeThenDeserializeAnExceptionAlwaysReturnOriginalException()
        {
            // Act.
            var serializedException = SerializeException(_exception);

            // Assert.
            var deserializedException = DeserializeException(serializedException);

            Assert.Equal(_requestUri, deserializedException.RequestUri);
            Assert.Equal(_statusCode, deserializedException.StatusCode);
            Assert.Equal(_failedReason, deserializedException.FailedReason);
        }

        private static string SerializeException(RemotingRefException ex)
        {
            return JsonSerializer.Serialize(ex);
        }

        private static RemotingRefException DeserializeException(string json)
        {
            return JsonSerializer.Deserialize<RemotingRefException>(json);
        }
    }
}