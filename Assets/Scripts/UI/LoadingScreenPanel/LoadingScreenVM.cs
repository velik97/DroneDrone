using System.Collections;
using SceneLoading;
using UI.MVVM;
using UniRx;
using UnityEngine;
using Util.CoroutineHandler;
using Util.EventBusSystem;

namespace UI.LoadingScreenPanel
{
    public class LoadingScreenVM : ViewModel, IAsyncSceneLoadingProcessHandler
    {
        public FloatReactiveProperty LoadingProcess = new FloatReactiveProperty();
        public ReactiveCommand OnOpen = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();

        private const float TRANSITION_VALUE = 0.8f;
        private const float LOAD_TIME = 0.25f;
        private const float MIN_PROCESS_VALUE = 0.05f;
        private static bool s_FirstScene = true;
        
        private bool m_IsOpened = false;
        public bool IsOpened => m_IsOpened;

        public LoadingScreenVM()
        {
            AddDisposable(EventBus.Subscribe(this));
            if (s_FirstScene)
            {
                s_FirstScene = false;
                return;
            }

            CoroutineHandler.StartCoroutineOnHandler(StartLoading());
        }

        public void HandleAsyncSceneLoadingProcess(float value)
        {
            if (!m_IsOpened)
            {
                Open();
            }
            LoadingProcess.Value = Mathf.Max(value * TRANSITION_VALUE, MIN_PROCESS_VALUE);
        }

        private void Close()
        {
            if (!m_IsOpened)
            {
                return;
            }

            m_IsOpened = false;
            OnClose.Execute();
        }

        private void Open()
        {
            if (m_IsOpened)
            {
                return;
            }

            m_IsOpened = true;
            OnOpen.Execute();
        }

        private IEnumerator StartLoading()
        {
            Open();
            float startTime = Time.time;
            float t;

            while ((t = (Time.time - startTime)/LOAD_TIME) < 1f)
            {
                LoadingProcess.Value = TRANSITION_VALUE + (1 - TRANSITION_VALUE) * t;
                yield return null;
            }

            LoadingProcess.Value = 1f;
            Close();
        }
    }
}