using System;
using UI.ChooseLevelMenu;
using UI.LoadingScreenPanel;
using UnityEngine;
using Util;

namespace UI.Config
{
    public class MainMenuUiConfig : DisposableContainerMonoBehaviour
    {
        [SerializeField]
        private ChooseLevelMenuView m_ChooseLevelMenuView;
        [SerializeField]
        private LoadingScreenView m_LoadingScreenView;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            AddDisposable(m_ChooseLevelMenuView.CreateViewModelAndBind());
            AddDisposable(m_LoadingScreenView.CreateViewModelAndBind());
        }
    }
}