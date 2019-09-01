using DefaultNamespace;
using UI.BadSignalWarningMenu;
using UI.FinishIndicator;
using UI.GameOverPanel;
using UI.PauseMenu;
using UI.WinMenu;
using UnityEngine;
using Util;

namespace UI.Config
{
    public class InGameConfig : MonoSingleton<InGameConfig>
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

        public QuickRestartView QuickRestartView => m_QuickRestartView;
        public PauseMenuView PauseMenuView => m_PauseMenuView;
        public GameOverPanelView GameOverPanelView => m_GameOverPanelView;
        public BadSignalWarningView BadSignalWarningView => m_BadSignalWarningView;
        public WinMenuView WinMenuView => m_WinMenuView;
        
        private FinishIndicatorView m_FinishIndicatorView;
        public FinishIndicatorView FinishIndicatorView
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
    }
}