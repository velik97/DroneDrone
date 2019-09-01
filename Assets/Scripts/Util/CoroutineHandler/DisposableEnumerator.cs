using System;
using System.Collections;

namespace Util.CoroutineHandler
{
    public class DisposableEnumerator : IDisposable, IEnumerator
    {
        private readonly IEnumerator m_EnumeratorWithCheck;
        private readonly Action<IEnumerator> m_DisposeAction;
        
        private bool m_IsRunning = false;
        private bool m_IsDisposed = false;

        public DisposableEnumerator(IEnumerator enumerator, Action<IEnumerator> disposeAction)
        {
            m_EnumeratorWithCheck = EnumeratorWithCheck(enumerator);
            m_DisposeAction = disposeAction;
        }

        private IEnumerator EnumeratorWithCheck(IEnumerator enumerator)
        {
            m_IsRunning = true;
            yield return enumerator;
            m_IsRunning = false;
        }

        public void Dispose()
        {
            if (m_IsDisposed)
            {
                return;
            }
            m_IsDisposed = true;
            
            if (m_IsRunning)
            {
                m_DisposeAction?.Invoke(this);
            }
        }

        public bool MoveNext()
        {
            return m_EnumeratorWithCheck.MoveNext();
        }

        public void Reset()
        {
            m_EnumeratorWithCheck.Reset();
        }

        public object Current => m_EnumeratorWithCheck.Current;
    }
}