namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Delegates;
    using Fanex.RemotingRef.TranslationStorage;

    public class RefDataTypeContainer
    {
        private readonly Dictionary<RefDataType, RefDataTypeHandlerOptions> _registeredDataTypeHandlerOptions;
        private readonly ConcurrentDictionary<RefDataType, IRefDataTypeHandler> _registeredDataTypeHandlers;
        private readonly Uri _remotingRefServiceUri;

        private RefDataTypeContainer(Uri remotingRefServiceUri)
        {
            _registeredDataTypeHandlerOptions = PreRegisterSpecialDataTypes();
            _registeredDataTypeHandlers = new ConcurrentDictionary<RefDataType, IRefDataTypeHandler>();
            _remotingRefServiceUri = remotingRefServiceUri;
        }

        private static RefDataTypeContainer CurrentInstance { get; set; }

        internal static Uri ServiceUri => CurrentInstance._remotingRefServiceUri;

        private Dictionary<RefDataType, RefDataTypeHandlerOptions> PreRegisterSpecialDataTypes()
        {
            var registeredOptions = new Dictionary<RefDataType, RefDataTypeHandlerOptions>
            {
                {
                    RefDataType.TeamName,
                    new RefDataTypeHandlerOptions
                    {
                        CompactOnMemoryPressure = true,
                        NumberOfEntriesLimit = 1000000,
                    }
                },
                {
                    RefDataType.LeagueName,
                    new RefDataTypeHandlerOptions
                    {
                        CompactOnMemoryPressure = true,
                        NumberOfEntriesLimit = 17000,
                    }
                }
            };

            return registeredOptions;
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

        public static IRefDataTypeHandler TryResolveDataTypeHandler(
            RefDataType dataType,
            RequestTranslationSetDelegate valueFactory,
            SyncTranslationSetsDelegate syncDataHandler)
        {
            if (CurrentInstance._registeredDataTypeHandlers.TryGetValue(dataType, out IRefDataTypeHandler existingHandler))
            {
                return existingHandler;
            }

            RefDataTypeHandlerOptions options = TryResolveDataTypeHandlerOptions(dataType);

            options.ValueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            options.SyncHandler = syncDataHandler ?? throw new ArgumentNullException(nameof(syncDataHandler));

            var newHandler = new RefDataTypeHandler(options, new LocalStorage(options, new SystemClock()));

            if (CurrentInstance._registeredDataTypeHandlers.TryAdd(dataType, newHandler))
            {
                return newHandler;
            }

            if (CurrentInstance._registeredDataTypeHandlers.TryGetValue(dataType, out existingHandler))
            {
                return existingHandler;
            }

            return newHandler;
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

            if (_registeredDataTypeHandlerOptions.ContainsKey(dataType))
            {
                _registeredDataTypeHandlerOptions[dataType] = options;
            }
            else
            {
                _registeredDataTypeHandlerOptions.Add(dataType, options);
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

        public RefDataTypeContainer RegisterSupportedLanguages(string[] languageCodes)
        {
            RefDataTypeHandlerOptions.SupportedLanguageCodes = languageCodes;

            return this;
        }
    }
}