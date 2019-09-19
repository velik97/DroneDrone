using System;

namespace Util
{
    public class DisposeAction : IDisposable
    {
        private readonly Action m_Dispose;

        public DisposeAction(Action dispose)
        {
            m_Dispose = dispose;
        }

        public void Dispose()
        {
            m_Dispose();
        }
    }
}