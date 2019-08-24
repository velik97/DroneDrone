using DefaultNamespace;
using UI.PauseMenu;
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

        public QuickRestartView QuickRestartView => m_QuickRestartView;
        public PauseMenuView PauseMenuView => m_PauseMenuView;
    }
}