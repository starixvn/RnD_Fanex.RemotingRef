namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    using AutoFixture;
    using Xunit;

    public class RemotingRefExceptionTests
    {
        private readonly string _request;
        private readonly HttpStatusCode _statusCode;
        private readonly string _failedReason;
        private readonly RemotingRefException _exception;

        public RemotingRefExceptionTests()
        {
            var fixture = new Fixture();

            _request = fixture.Create<string>();
            _statusCode = fixture.Create<HttpStatusCode>();
            _failedReason = fixture.Create<string>();
            _exception = new RemotingRefException(_request, _statusCode, _failedReason);
        }

        [Fact]
        public void SerializeException_Always_NotThrowException()
        {
            // Act.
            Exception unexpectedException = Record.Exception(() => SerializeException(_exception));

            // Assert.
            Assert.Null(unexpectedException);
        }

        [Fact]
        public void SerializeException_Always_BeSuccessful()
        {
            // Act.
            byte[] actualResult = SerializeException(_exception);

            // Assert.
            Assert.NotEmpty(actualResult);
        }

        [Fact]
        public void SerializeAndDeserializeException_Always_ReturnOriginalExceptionAfterDeserializing()
        {
            // Act.
            byte[] serializedException = SerializeException(_exception);

            // Assert.
            RemotingRefException deserializedException = DeserializeException(serializedException);

            Assert.Equal(_request, deserializedException.Request);
            Assert.Equal(_statusCode, deserializedException.StatusCode);
            Assert.Equal(_failedReason, deserializedException.FailedReason);
        }

        private static byte[] SerializeException(RemotingRefException ex)
        {
            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, ex);

                return memoryStream.ToArray();
            }
        }

        private static RemotingRefException DeserializeException(byte[] stream)
        {
            var binaryFormatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream(stream))
            {
                return binaryFormatter.Deserialize(memoryStream) as RemotingRefException;
            }
        }
    }
}