using UI.ChooseLevelMenu;
using UnityEngine;
using Util;

namespace UI.Config
{
    public class MainMenuConfig : MonoSingleton<MainMenuConfig>
    {
        [SerializeField]
        private ChooseLevelMenuView m_ChooseLevelMenuView;

        public ChooseLevelMenuView ChooseLevelMenuView => m_ChooseLevelMenuView;
    }
}