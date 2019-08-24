using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.MVVM
{
    public abstract class View<TViewModel> : MonoBehaviour where TViewModel : ViewModel
    {
        private bool m_IsDisposed = false;
        public bool IsDisposed => m_IsDisposed;

        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();
        
        protected TViewModel ViewModel;

        public virtual void Bind(TViewModel viewModel)
        {
            Unbind();
            ViewModel = viewModel;
            ViewModel.OnDispose += Dispose;
        }

        private void Unbind()
        {
            if (ViewModel == null)
            {
                return;
            }
            ViewModel.OnDispose -= Dispose;
        }

        public void AddDisposable(IDisposable disposable)
        {
            if (m_IsDisposed || m_Disposables.Contains(disposable))
            {
                return;
            }
            m_Disposables.Add(disposable);
        }

        private void Dispose()
        {
            if (m_IsDisposed)
            {
                Debug.LogError($"[{GetType()}] is already disposed, but you are trying to do it again!");
                return;
            }
            Unbind();

            m_IsDisposed = true;
            
            foreach (IDisposable disposable in m_Disposables)
            {
                disposable.Dispose();
            }
            
            m_Disposables.Clear();
            DestroyViewImplementation();
        }

        protected abstract void DestroyViewImplementation();

    }
}