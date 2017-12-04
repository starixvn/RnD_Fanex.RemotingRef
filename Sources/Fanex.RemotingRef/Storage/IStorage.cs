namespace Fanex.RemotingRef.Storage
{
    using System;

    public interface IStorage
    {
        TimeSpan TimeToLive { get; }

        bool Contain(string key);

        bool TryGet(string key, out object value);

        void Set(string key, object value);
    }
}