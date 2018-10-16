namespace Fanex.RemotingRef.Service.Tests.Formatters
{
    using System;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Formatters;
    using MessagePack;
    using MessagePack.Resolvers;
    using Xunit;

    public class MessagePackFormatterTests
    {
        private readonly MessagePackFormatter _formatter;
        private readonly Fixture _fixture;

        public MessagePackFormatterTests()
        {
            _formatter = new MessagePackFormatter();
            _fixture = new Fixture();
        }

        [Fact]
        public void RegisterMsgPackAsMimeType()
        {
            // Assert.
            var expectedMimeType = new MediaTypeHeaderValue("application/x-msgpack");

            Assert.Contains(expectedMimeType, _formatter.SupportedMediaTypes);
        }

        [Fact]
        public void CanReadAnyType()
        {
            // Arrange.
            var anyType = _fixture.Create<Type>();

            // Act.
            var readable = _formatter.CanReadType(anyType);

            // Assert.
            Assert.True(readable);
        }

        [Fact]
        public void CanWriteAnyType()
        {
            // Arrange.
            var anyType = _fixture.Create<Type>();

            // Act.
            var writable = _formatter.CanWriteType(anyType);

            // Assert.
            Assert.True(writable);
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionIfWriteToStreamAsyncTakesNullAsType()
        {
            // Arrange.
            Type type = null;
            var value = _fixture.Create<string>();
            var writeStream = new MemoryStream();

            // Act.
            var exception = await Record.ExceptionAsync(() => _formatter.WriteToStreamAsync(
                type,
                value,
                writeStream,
                null,
                null));

            writeStream.Dispose();

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionIfWriteToStreamAsyncTakesNullAsWriteStream()
        {
            // Arrange.
            var type = typeof(string);
            var value = _fixture.Create<string>();
            Stream writeStream = null;

            // Act.
            var exception = await Record.ExceptionAsync(() => _formatter.WriteToStreamAsync(
                type,
                value,
                writeStream,
                null,
                null));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task WriteToStreamAsyncUsesMessagePackToSerializeGivenValue()
        {
            // Arrange.
            var type = typeof(string);
            var value = _fixture.Create<string>();
            var writeStream = new MemoryStream();

            // Act.
            await _formatter.WriteToStreamAsync(
                type,
                value,
                writeStream,
                null,
                null);

            byte[] actualBinary = writeStream.ToArray();

            writeStream.Dispose();

            // Assert.
            var expectedBinary = MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);

            Assert.Equal(expectedBinary, actualBinary);
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionIfReadFromStreamAsyncTakesNullAsType()
        {
            // Arrange.
            Type type = null;
            var readStream = new MemoryStream();

            // Act.
            var exception = await Record.ExceptionAsync(() => _formatter.ReadFromStreamAsync(
                type,
                readStream,
                null,
                null));

            readStream.Dispose();

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionIfReadFromStreamAsyncTakesNullAsWriteStream()
        {
            // Arrange.
            var type = typeof(string);
            Stream readStream = null;

            // Act.
            var exception = await Record.ExceptionAsync(() => _formatter.ReadFromStreamAsync(
                type,
                readStream,
                null,
                null));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public async Task ReadFromStreamAsyncUsesMessagePackToDeserializeGivenValue()
        {
            // Arrange.
            var type = typeof(string);
            var value = _fixture.Create<string>();
            var binary = MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);
            var readStream = new MemoryStream(binary);

            // Act.
            var actualValue = await _formatter.ReadFromStreamAsync(
                type,
                readStream,
                null,
                null);

            readStream.Dispose();

            // Assert.
            Assert.Equal(value, actualValue);
        }
    }
}