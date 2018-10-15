namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
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

        protected static readonly string RequestUrlPattern = $"api/ref/{{0}}&languages=en-US&applicationName={ApplicationName}";

        private bool _disposed;

        protected RemotingRefServiceTestBase()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri("http://localhost:10000"))
                .FromApplication(ApplicationName)
                .SupportOnlyDefaultLanguage();

            HttpClient = Substitute.For<IHttpHandler>();
            Serializer = Substitute.For<ISerializer>();
            RemotingRefService = new RemotingRefService(HttpClient, Serializer);
            Fixture = new Fixture();

            HttpClient
                .GetAsync(Arg.Any<string>())
                .Returns(FakeHttpResponseMessage());
        }

        protected IHttpHandler HttpClient { get; }

        protected ISerializer Serializer { get; }

        protected IRemotingRefService RemotingRefService { get; }

        protected Fixture Fixture { get; }

        private static HttpResponseMessage FakeHttpResponseMessage()
            => new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream(new byte[] { }))
            };

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RefDataTypeContainer.Reset();
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