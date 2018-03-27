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
                if (CurrentInstance == null)
                {
                    throw new InvalidOperationException($"Method '{nameof(RefDataTypeContainer.ConnectTo)}' has not been called.");
                }

                return CurrentInstance._remotingRefServiceUri;
            }
        }

        private static RefDataTypeContainer CurrentInstance { get; set; }

        private void PreRegisterSpecialDataTypes()
        {
            _registeredDataTypeHandlerOptions.TryAdd(
                RefDataType.TeamName,
                new RefDataTypeHandlerOptions { CompactOnMemoryPressure = true, NumberOfEntriesLimit = 100_000 });

            _registeredDataTypeHandlerOptions.TryAdd(
                RefDataType.LeagueName,
                new RefDataTypeHandlerOptions { CompactOnMemoryPressure = true, NumberOfEntriesLimit = 17_000 });
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
            => CurrentInstance = null;

        internal static IRefDataTypeHandler TryResolveDataTypeHandler(
            RefDataType dataType,
            RequestTranslationSetDelegate valueFactory,
            SyncTranslationSetsDelegate syncDataHandler)
        {
            IRefDataTypeHandler handler = CurrentInstance._registeredDataTypeHandlers.GetOrAdd(
                dataType,
                checkingDataType => CreateRefDataHandler(checkingDataType, valueFactory, syncDataHandler));

            return handler;
        }

        private static IRefDataTypeHandler CreateRefDataHandler(
            RefDataType dataType,
            RequestTranslationSetDelegate valueFactory,
            SyncTranslationSetsDelegate syncDataHandler)
        {
            RefDataTypeHandlerOptions options = TryResolveDataTypeHandlerOptions(dataType);

            options.ValueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            options.SyncHandler = syncDataHandler ?? throw new ArgumentNullException(nameof(syncDataHandler));

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
            RefDataTypeHandlerOptions.SupportedLanguageCodes = languageCodes;

            return this;
        }
    }
}