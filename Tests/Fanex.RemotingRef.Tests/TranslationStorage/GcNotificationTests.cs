namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Threading;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class GcNotificationTests : IDisposable
    {
        private ManualResetEventSlim _callbackInvoked;

        public GcNotificationTests()
            => _callbackInvoked = new ManualResetEventSlim(false);

        [Fact]
        public void CallbackIsRegisteredAndInvoked()
        {
            // Arrange.
            InvokeInItsOwnScope(() =>
            {
                GcNotification.Register(
                    state =>
                    {
                        _callbackInvoked.Set();

                        return false;
                    },
                    null);
            });

            // Act.
            GcCollectAndWait();

            // Assert.
            Assert.True(_callbackInvoked.Wait(0));
        }

        [Fact]
        public void CallbackWillNotBeInvokedIfReturnsFalseAtThePreviousInvocation()
        {
            // Arrange.
            InvokeInItsOwnScope(() =>
            {
                GcNotification.Register(
                    state =>
                    {
                        _callbackInvoked.Set();

                        return false;
                    },
                    null);
            });
            GcCollectAndWait();

            // Make sure the callback is invoked at the first time.
            Assert.True(_callbackInvoked.Wait(0));

            // Act.
            _callbackInvoked.Reset();
            GcCollectAndWait();

            // Assert.
            Assert.False(_callbackInvoked.Wait(0));
        }

        [Fact]
        public void KeepInvokingCallbackIfItReturnsTrueAtPreviousInvocation()
        {
            // Arrange.
            InvokeInItsOwnScope(() =>
            {
                GcNotification.Register(
                    state =>
                    {
                        _callbackInvoked.Set();

                        return true;
                    },
                    null);
            });
            GcCollectAndWait();

            // Make sure the callback is invoked at the first time.
            Assert.True(_callbackInvoked.Wait(0));

            // Act.
            _callbackInvoked.Reset();
            GcCollectAndWait();

            // Assert.
            Assert.True(_callbackInvoked.Wait(0));
        }

        [Fact]
        public void RetryInvokingCallbackIfExceptionWasThrownAtPreviousCall()
        {
            // Arrange.
            int invocationCount = 0;

            InvokeInItsOwnScope(() =>
            {
                GcNotification.Register(
                    state =>
                    {
                        if (invocationCount == 0)
                        {
                            invocationCount++;

                            throw new InvalidOperationException();
                        }

                        _callbackInvoked.Set();

                        return false;
                    },
                    null);
            });
            GcCollectAndWait();

            // Act.
            GcCollectAndWait();

            // Assert.
            Assert.True(_callbackInvoked.Wait(0));
        }

#pragma warning disable S1215 // "GC.Collect" should not be called

        private static void GcCollectAndWait()
        {
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

#pragma warning restore S1215 // "GC.Collect" should not be called

        // If the code is built with 'Debug' mode or the tests are run by NCrunch, which disables some compiler optimizations,
        // the Garbage Collector is not triggered as expected or as what it should be at the runtime.
        // So that we must wrap actions related to GcNotification.Register in its own scope to make NCrunch be happy.
        private static void InvokeInItsOwnScope(Action action)
            => action();

        public void Dispose()
        {
            _callbackInvoked.Dispose();
            _callbackInvoked = null;
        }
    }
}