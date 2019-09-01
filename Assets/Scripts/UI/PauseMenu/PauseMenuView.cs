using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PauseMenu
{
    public class PauseMenuView : View<PauseMenuVM>
    {
        [SerializeField]
        private Button m_InGamePauseButton;

        [SerializeField]
        private GameObject m_MenuObject;
        
        [SerializeField]
        private Button m_ContinueButton;
        [SerializeField]
        private Button m_RestartButton;
        [SerializeField]
        private Button m_GoToMainMenuButtonButton;

        public override void Bind(PauseMenuVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(m_InGamePauseButton.onClick.AsObservable().Subscribe(_ => viewModel.PauseGame()));
            AddDisposable(m_ContinueButton.onClick.AsObservable().Subscribe(_ => viewModel.UnPauseGame()));
            
            AddDisposable(m_RestartButton.onClick.AsObservable().Subscribe(_ => viewModel.RestartGame()));
            AddDisposable(m_GoToMainMenuButtonButton.onClick.AsObservable().Subscribe(_ => viewModel.GoToMainMenu()));
            
            AddDisposable(m_InGamePauseButton.onClick.AsObservable().Subscribe(_ => OpenMenu()));
            AddDisposable(m_ContinueButton.onClick.AsObservable().Subscribe(_ => CloseMenu()));
            
            gameObject.SetActive(true);
            m_MenuObject.SetActive(false);
        }

        private void OpenMenu()
        {
            m_InGamePauseButton.interactable = false;
            m_MenuObject.SetActive(true);
        }

        private void CloseMenu()
        {
            m_InGamePauseButton.interactable = true;
            m_MenuObject.SetActive(false);
        }

        protected override void DestroyViewImplementation()
        {
            gameObject.SetActive(false);
        }
    }
}