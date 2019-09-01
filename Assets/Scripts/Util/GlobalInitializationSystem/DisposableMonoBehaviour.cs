using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util.GlobalInitializationSystem
{
    public class DisposableMonoBehaviour : MonoBehaviour
    {
        private event Action OnDisposeInternal;
        public event Action OnDispose
        {
            add { OnDisposeInternal += value; }
            remove { OnDisposeInternal -= value; }
        }
        
        private bool m_IsDisposed = false;
        public bool IsDisposed => m_IsDisposed;

        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();

        public void AddDisposable(IDisposable disposable)
        {
            if (m_IsDisposed || m_Disposables.Contains(disposable))
            {
                return;
            }
            m_Disposables.Add(disposable);
        }

        public void Dispose()
        {
            if (m_IsDisposed)
            {
                return;
            }

            m_IsDisposed = true;
            
            foreach (IDisposable disposable in m_Disposables)
            {
                disposable.Dispose();
            }

            OnDisposeInternal?.Invoke();
            m_Disposables.Clear();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}