using UI.ChooseLevelMenu;
using UI.Config;
using Util;
using Util.GlobalInitializationSystem;

namespace UnityEngine.UI
{
    public class MainMenuContext : BaseDisposable, IGlobalInitializableInMainMenu
    {
        private ChooseLevelMenuVM m_ChooseLevelMenuVm;

        
        public InitializePrior InitializePrior => InitializePrior.UsualStart;
        public void Initialize()
        {
            InitializeChooseLevelMenu();
        }
        
        private void InitializeChooseLevelMenu()
        {
            m_ChooseLevelMenuVm = new ChooseLevelMenuVM();
            MainMenuConfig.Instance.ChooseLevelMenuView.Bind(m_ChooseLevelMenuVm);
            AddDisposable(m_ChooseLevelMenuVm);
        }
    }
}