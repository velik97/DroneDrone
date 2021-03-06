using System;
using DefaultNamespace;
using UI.BadSignalWarningMenu;
using UI.DevMenu;
using UI.FinishIndicator;
using UI.GameOverPanel;
using UI.LoadingScreenPanel;
using UI.PauseMenu;
using UI.TouchControl;
using UI.WinMenu;
using UnityEngine;
using Util;

namespace UI.Config
{
    public class InGameUiConfig : DisposableContainerMonoBehaviour
    {
        [SerializeField]
        private QuickRestartView m_QuickRestartView;
        [SerializeField]
        private PauseMenuView m_PauseMenuView;
        [SerializeField]
        private GameOverPanelView m_GameOverPanelView;
        [SerializeField]
        private BadSignalWarningView m_BadSignalWarningView;
        [SerializeField]
        private WinMenuView m_WinMenuView;
        [SerializeField]
        private LoadingScreenView m_LoadingScreenView;
        [SerializeField]
        private DevMenuView m_DevMenuView;
        [SerializeField]
        private TouchControlView m_TouchControlView;
        

        private FinishIndicatorView m_FinishIndicatorView;
        
        private FinishIndicatorView FinishIndicatorView
        {
            get
            {
                if (m_FinishIndicatorView == null)
                {
                    m_FinishIndicatorView = FindObjectOfType<FinishIndicatorView>();
                }

                return m_FinishIndicatorView;
            }
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            AddDisposable(m_QuickRestartView.CreateViewModelAndBind());
            AddDisposable(m_PauseMenuView.CreateViewModelAndBind());
            AddDisposable(m_GameOverPanelView.CreateViewModelAndBind());
            AddDisposable(m_BadSignalWarningView.CreateViewModelAndBind());
            AddDisposable(m_WinMenuView.CreateViewModelAndBind());
            AddDisposable(m_LoadingScreenView.CreateViewModelAndBind());

            AddDisposable(FinishIndicatorView.CreateViewModelAndBind());
            
#if PLATFORM_IOS || PLATFORM_ANDROID
            AddDisposable(m_TouchControlView.CreateViewModelAndBind());
#endif

            if (Debug.isDebugBuild)
            {
                AddDisposable(m_DevMenuView.CreateViewModelAndBind());
            }
            else
            {
                Destroy(m_DevMenuView);
            }
        }
    }
}