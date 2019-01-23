namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using AutoFixture;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Fanex.RemotingRef.Utilities.WebClient;
    using NSubstitute;
    using Xunit;

    [Collection("SharedContainer")]
    public class RemotingRefServiceTestBase : IDisposable
    {
        protected const string ApplicationName = "UnitTest_AppName";
        protected const string TestingLanguageCode = RefDataTypeHandlerOptions.DefaultLanguage;

        private bool _disposed;

        protected RemotingRefServiceTestBase()
        {
            var version = RefDataTypeContainer.GetVersion();

            RefDataTypeContainer
                .ConnectTo(new Uri("http://localhost:10000"))
                .FromApplication(ApplicationName)
                .SupportOnlyDefaultLanguage();

            RequestRelativePathPattern = $"api/ref/{{0}}&languages=en-US&applicationName={ApplicationName}-{version}";
            HttpClient = Substitute.For<IHttpHandler>();
            Serializer = new MessagePackSerializer();
            RemotingRefService = new RemotingRefService(HttpClient, Serializer);
            Response = new HttpResponseMessage();
            Fixture = new Fixture();
        }

        protected string RequestRelativePathPattern { get; }

        protected string ExpectedRequestRelativePath { get; set; }

        protected IHttpHandler HttpClient { get; }

        protected ISerializer Serializer { get; }

        protected IRemotingRefService RemotingRefService { get; }

        protected HttpResponseMessage Response { get; }

        protected Fixture Fixture { get; }

        protected static StreamContent FakeResponseContent(byte[] binary)
            => new StreamContent(new MemoryStream(binary));

        protected byte[] SerializeTranslationSet<T>(T value)
        {
            var translationSet = new Dictionary<string, object>
            {
                [TestingLanguageCode] = value
            };

            return Serializer.Serialize(translationSet);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RefDataTypeContainer.Reset();
                    Response.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}