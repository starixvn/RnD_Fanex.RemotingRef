namespace Fanex.RemotingRef
{
    using System.Collections.Generic;
    using Fanex.RemotingRef.TranslationStorage;

    internal class RequestSyncFormData
    {
        internal IList<KeySet> ParameterSets { get; set; }

        internal string[] Languages { get; set; }
    }
}