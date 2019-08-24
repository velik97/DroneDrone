using DefaultNamespace;
using UI.Config;
using UI.PauseMenu;
using Util.GlobalInitializationSystem;

namespace UnityEngine.UI
{
    public class InGameContext : IGlobalInitializableInGame
    {
        private QuickRestartVM m_QuickRestartVm;
        private PauseMenuVM m_PauseMenuVm;
            
        public InGameContext()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeQuickRestartMenu();
            InitializePauseMenu();
        }

        private void InitializeQuickRestartMenu()
        {
            m_QuickRestartVm = new QuickRestartVM();
            InGameConfig.Instance.QuickRestartView.Bind(m_QuickRestartVm);
        }
        
        private void InitializePauseMenu()
        {
            m_PauseMenuVm = new PauseMenuVM();
            InGameConfig.Instance.PauseMenuView.Bind(m_PauseMenuVm);
        }

        public void Dispose()
        {
            if (!m_QuickRestartVm.IsDisposed)
            {
                m_QuickRestartVm.Dispose();
            }
            if (!m_PauseMenuVm.IsDisposed)
            {
                m_PauseMenuVm.Dispose();
            }
        }
    }
}