namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Concurrent;
    using Fanex.RemotingRef.Delegates;
    using Fanex.RemotingRef.TranslationStorage;

    public class RefDataTypeContainer
    {
        private readonly ConcurrentDictionary<RefDataType, RefDataTypeHandlerOptions> _registeredDataTypeHandlerOptions;
        private readonly ConcurrentDictionary<RefDataType, IRefDataTypeHandler> _registeredDataTypeHandlers;
        private readonly Uri _remotingRefServiceUri;

        private static Uri _serviceUri;

        private RefDataTypeContainer(Uri remotingRefServiceUri)
        {
            _registeredDataTypeHandlerOptions = new ConcurrentDictionary<RefDataType, RefDataTypeHandlerOptions>();
            _registeredDataTypeHandlers = new ConcurrentDictionary<RefDataType, IRefDataTypeHandler>();
            _remotingRefServiceUri = remotingRefServiceUri;

            PreRegisterSpecialDataTypes();
        }

        public static Uri ServiceUri
        {
            get
            {
                if (_serviceUri == null)
                {
                    VerifyConfiguration();

                    _serviceUri = CurrentInstance._remotingRefServiceUri;
                }

                return _serviceUri;
            }
        }

        private static RefDataTypeContainer CurrentInstance { get; set; }

        private void PreRegisterSpecialDataTypes()
        {
            Register(RefDataType.TeamName, new RefDataTypeHandlerOptions { CompactOnMemoryPressure = true, NumberOfEntriesLimit = 100_000 });
            Register(RefDataType.LeagueName, new RefDataTypeHandlerOptions { CompactOnMemoryPressure = true, NumberOfEntriesLimit = 17_000 });
            Register(RefDataType.BetTypeName, new RefDataTypeHandlerOptions { NumberOfEntriesLimit = 2_000 });
            Register(RefDataType.DisplayedBetTypeName, new RefDataTypeHandlerOptions { NumberOfEntriesLimit = 2_000 });
        }

        public static RefDataTypeContainer ConnectTo(Uri remotingRefServiceUri)
        {
            if (remotingRefServiceUri == null)
            {
                throw new ArgumentNullException(nameof(remotingRefServiceUri));
            }

            CurrentInstance = new RefDataTypeContainer(remotingRefServiceUri);

            return CurrentInstance;
        }

        internal static void Reset()
        {
            _serviceUri = null;
            CurrentInstance = null;
            RefDataTypeHandlerOptions.ApplicationName = null;
            RefDataTypeHandlerOptions.IsMultiLanguagesSupported = true;
            RefDataTypeHandlerOptions.SupportedLanguageCodes = RefDataTypeHandlerOptions.DefaultSupportedLanguageCodes;
        }

        internal static IRefDataTypeHandler TryResolveDataTypeHandler(
            RefDataType dataType,
            RequestTranslationSetDelegate requestHandler,
            SyncTranslationSetsDelegate syncHandler)
        {
            var handler = CurrentInstance._registeredDataTypeHandlers.GetOrAdd(
                dataType,
                checkingDataType => CreateRefDataHandler(checkingDataType, requestHandler, syncHandler));

            return handler;
        }

        private static IRefDataTypeHandler CreateRefDataHandler(
            RefDataType dataType,
            RequestTranslationSetDelegate requestHandler,
            SyncTranslationSetsDelegate syncHandler)
        {
            var options = TryResolveDataTypeHandlerOptions(dataType);

            options.RequestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
            options.SyncHandler = syncHandler ?? throw new ArgumentNullException(nameof(syncHandler));

            return new RefDataTypeHandler(options, new LocalStorage(options, new SystemClock()));
        }

        private static RefDataTypeHandlerOptions TryResolveDataTypeHandlerOptions(RefDataType dataType)
        {
            if (CurrentInstance._registeredDataTypeHandlerOptions.TryGetValue(dataType, out RefDataTypeHandlerOptions options))
            {
                return options;
            }

            return new RefDataTypeHandlerOptions();
        }

        public RefDataTypeContainer Register(RefDataType dataType, RefDataTypeHandlerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _registeredDataTypeHandlerOptions.AddOrUpdate(
                dataType,
                options,
                (updatingDataType, oldOptions) => options);

            return this;
        }

        public RefDataTypeContainer FromApplication(string applicationName)
        {
            if (!string.IsNullOrEmpty(applicationName))
            {
                RefDataTypeHandlerOptions.ApplicationName = $"{applicationName}-{GetVersion()}";
            }

            return this;
        }

        public RefDataTypeContainer SupportMultiLanguages()
        {
            RefDataTypeHandlerOptions.IsMultiLanguagesSupported = true;

            return this;
        }

        public RefDataTypeContainer SupportOnlyDefaultLanguage()
        {
            RefDataTypeHandlerOptions.IsMultiLanguagesSupported = false;

            return this;
        }

        public RefDataTypeContainer RegisterSupportedLanguages(params string[] languageCodes)
        {
            if (languageCodes?.Length > 0)
            {
                RefDataTypeHandlerOptions.SupportedLanguageCodes = languageCodes;
            }

            return this;
        }

        internal static Version GetVersion()
            => typeof(RefDataTypeContainer).Assembly.GetName().Version;

        private static void VerifyConfiguration()
        {
            if (CurrentInstance == null)
            {
                throw new InvalidOperationException(
                    $"URL to Fanex.RemotingRef.Service must be configured by calling '{nameof(RefDataTypeContainer.ConnectTo)}'.");
            }

            if (RefDataTypeHandlerOptions.ApplicationName == null)
            {
                throw new InvalidOperationException(
                    $"Application Name must be provided via '{nameof(RefDataTypeContainer.FromApplication)}' for purpose of statistics.");
            }
        }
    }
}