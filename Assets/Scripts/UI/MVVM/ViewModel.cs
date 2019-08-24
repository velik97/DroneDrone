using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.MVVM
{
    public abstract class ViewModel : IDisposable
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
                Debug.LogError($"[{GetType()}] is already disposed, but you are trying to do it again!");
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
    }
}