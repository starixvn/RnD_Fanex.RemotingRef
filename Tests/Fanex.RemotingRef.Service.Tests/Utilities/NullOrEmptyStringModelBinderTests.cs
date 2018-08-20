namespace Fanex.RemotingRef.Service.Tests.Utilities
{
    using System.Globalization;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata.Providers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ValueProviders;
    using AutoFixture;
    using Fanex.RemotingRef.Service.ModelBinders;
    using NSubstitute;
    using Xunit;

    public class NullOrEmptyStringModelBinderTests
    {
        private readonly HttpActionContext _actionContext;
        private readonly IValueProvider _valueProvider;
        private readonly ModelBindingContext _bindingContext;
        private readonly NullOrEmptyStringModelBinder _binder;
        private readonly Fixture _fixture;

        public NullOrEmptyStringModelBinderTests()
        {
            var modelMetadata = new DataAnnotationsModelMetadataProvider().GetMetadataForType(null, typeof(string));

            _valueProvider = Substitute.For<IValueProvider>();
            _actionContext = Substitute.For<HttpActionContext>();
            _bindingContext = new ModelBindingContext
            {
                ValueProvider = _valueProvider,
                ModelMetadata = modelMetadata
            };

            _binder = new NullOrEmptyStringModelBinder();
            _fixture = new Fixture();
        }

        [Fact]
        public void OnlyAcceptStringModelType()
        {
            // Act.
            var didBind = _binder.BindModel(_actionContext, _bindingContext);

            // Assert.
            Assert.True(didBind);
        }

        [Fact]
        public void TakeNoActionOnNonStringModelType()
        {
            // Arrange.
            _bindingContext.ModelMetadata = new DataAnnotationsModelMetadataProvider().GetMetadataForType(null, typeof(int));

            // Act.
            var didBind = _binder.BindModel(_actionContext, _bindingContext);

            // Assert.
            Assert.False(didBind);
        }

        [Fact]
        public void BindEmptyStringToModel()
        {
            // Arrange.
            var modelName = _fixture.Create<string>();

            _bindingContext.ModelName = modelName;
            _valueProvider
                .GetValue(Arg.Is(modelName))
                .Returns(new ValueProviderResult(string.Empty, string.Empty, CultureInfo.InvariantCulture));

            // Act.
            _binder.BindModel(_actionContext, _bindingContext);

            // Assert.
            Assert.Empty(_bindingContext.Model as string);
        }

        [Fact]
        public void BindMissingModelNameToNull()
        {
            // Arrange.
            var modelName = _fixture.Create<string>();
            var anotherModelName = _fixture.Create<string>();
            var anotherModelValue = _fixture.Create<string>();

            _bindingContext.ModelName = modelName;
            _valueProvider
                .GetValue(Arg.Is(anotherModelName))
                .Returns(new ValueProviderResult(anotherModelValue, string.Empty, CultureInfo.InvariantCulture));

            // Act.
            _binder.BindModel(_actionContext, _bindingContext);

            // Assert.
            Assert.Null(_bindingContext.Model as string);
        }

        [Fact]
        public void DoNotBindNonEmptyStringToModel()
        {
            // Arrange.
            var modelName = _fixture.Create<string>();
            var modelValue = _fixture.Create<string>();

            _bindingContext.ModelName = modelName;
            _valueProvider
                .GetValue(Arg.Is(modelName))
                .Returns(new ValueProviderResult(modelValue, string.Empty, CultureInfo.InvariantCulture));

            // Act.
            _binder.BindModel(_actionContext, _bindingContext);

            // Assert.
            Assert.Null(_bindingContext.Model);
        }
    }
}