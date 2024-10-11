namespace AWX.Cmdlets
{
    internal class Sleep : IDisposable
    {
        private ManualResetEvent? _waitHandle;
        private readonly object _syncObject = new();
        private bool _stopping = false;
        private bool _disposed = false;
        public Sleep() { }
        public void Do(int miliseconds)
        {
            lock (_syncObject)
            {
                if (!_stopping)
                {
                    _waitHandle = new ManualResetEvent(false);
                }
            }
            _waitHandle?.WaitOne(miliseconds, true);
        }
        public bool Stop()
        {
            if (_stopping) return false;
            _stopping = true;
            _waitHandle?.Set();
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_waitHandle is not null)
                {
                    _waitHandle.Dispose();
                    _waitHandle = null;
                }
                _disposed = true;
            }
        }
    }
}
