using DefaultNamespace;
using UI.BadSignalWarningMenu;
using UI.Config;
using UI.FinishIndicator;
using UI.GameOverPanel;
using UI.PauseMenu;
using UI.WinMenu;
using Util;
using Util.GlobalInitializationSystem;

namespace UnityEngine.UI
{
    public class InGameContext : DisposableContainer, IGlobalInitializableInGame
    {
        private QuickRestartVM m_QuickRestartVm;
        private PauseMenuVM m_PauseMenuVm;
        private GameOverPanelVM m_GameOverPanelVm;
        private FinishIndicatorVM m_FinishIndicatorVm;
        private BadSignalWarningVM m_BadSignalWarningVm;
        private WinMenuVM m_WinMenuVm;
            
        public InGameContext()
        {
        }

        public InitializePrior InitializePrior => InitializePrior.UsualStart;

        public void Initialize()
        {
            InitializeQuickRestartMenu();
            InitializePauseMenu();
            InitializeGameOverPanel();
            InitializeFinishIndicator();
            InitializeBadSignalWarning();
            InitializeWinMenu();
        }

        private void InitializeQuickRestartMenu()
        {
            m_QuickRestartVm = new QuickRestartVM();
            InGameConfig.Instance.QuickRestartView.Bind(m_QuickRestartVm);
            AddDisposable(m_QuickRestartVm);
        }
        
        private void InitializePauseMenu()
        {
            m_PauseMenuVm = new PauseMenuVM();
            InGameConfig.Instance.PauseMenuView.Bind(m_PauseMenuVm);
            AddDisposable(m_PauseMenuVm);
        }
        
        private void InitializeGameOverPanel()
        {
            m_GameOverPanelVm = new GameOverPanelVM();
            InGameConfig.Instance.GameOverPanelView.Bind(m_GameOverPanelVm);
            AddDisposable(m_GameOverPanelVm);
        }

        private void InitializeFinishIndicator()
        {
            m_FinishIndicatorVm = new FinishIndicatorVM();
            InGameConfig.Instance.FinishIndicatorView.Bind(m_FinishIndicatorVm);
            AddDisposable(m_FinishIndicatorVm);
        }
        
        private void InitializeBadSignalWarning()
        {
            m_BadSignalWarningVm = new BadSignalWarningVM();
            InGameConfig.Instance.BadSignalWarningView.Bind(m_BadSignalWarningVm);
            AddDisposable(m_BadSignalWarningVm);
        }
        
        private void InitializeWinMenu()
        {
            m_WinMenuVm = new WinMenuVM();
            InGameConfig.Instance.WinMenuView.Bind(m_WinMenuVm);
            AddDisposable(m_WinMenuVm);
        }
    }
}