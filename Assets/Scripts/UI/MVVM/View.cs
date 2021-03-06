using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.MVVM
{
    public abstract class View<TViewModel> : MonoBehaviour where TViewModel : ViewModel, new()
    {
        private bool m_IsDisposed = false;
        public bool IsDisposed => m_IsDisposed;

        private readonly List<IDisposable> m_Disposables = new List<IDisposable>();
        
        protected TViewModel ViewModel;

        public TViewModel CreateViewModelAndBind()
        {
            TViewModel vm = new TViewModel();
            Bind(vm);
            return vm;
        }

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

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (m_IsDisposed)
            {
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