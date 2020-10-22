// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Collections.Concurrent;

namespace Sample.IntegrationTests.Fixtures
{
    public abstract class ContextFixture : IDisposable
    {
        private readonly ConcurrentQueue<IDisposable> disposables;
        private bool isDisposed;

        public ContextFixture()
        {
            this.disposables = new ConcurrentQueue<IDisposable>();
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void AddDisposableObject(IDisposable disposable)
        {
            this.disposables.Enqueue(disposable);
        }

        protected abstract void Dispose(IDisposable disposable);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    while (this.disposables.Count > 0)
                    {
                        if (this.disposables.TryDequeue(out IDisposable disposable))
                        {
                            this.Dispose(disposable);
                            disposable.Dispose();
                        }
                    }
                }

                this.isDisposed = true;
            }
        }
    }
}
