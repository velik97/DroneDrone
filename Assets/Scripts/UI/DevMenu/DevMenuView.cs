using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DevMenu
{
    public class DevMenuView : View<DevMenuVM>
    {
        [SerializeField]
        private Button m_InGameOpenButton;

        [SerializeField]
        private GameObject m_MenuObject;
        
        [SerializeField]
        private Button m_ContinueButton;
        [SerializeField]
        private Button m_UnlockAllLevelsButton;
        [SerializeField]
        private Button m_LockAllLevelsButton;

        public override void Bind(DevMenuVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(m_InGameOpenButton.onClick.AsObservable().Subscribe(_ => viewModel.PauseGame()));
            AddDisposable(m_ContinueButton.onClick.AsObservable().Subscribe(_ => viewModel.UnPauseGame()));
            
            AddDisposable(m_InGameOpenButton.onClick.AsObservable().Subscribe(_ => OpenMenu()));
            AddDisposable(m_ContinueButton.onClick.AsObservable().Subscribe(_ => CloseMenu()));

            AddDisposable(m_UnlockAllLevelsButton.onClick.AsObservable().Subscribe(_ => viewModel.UnlockAllLevels()));
            AddDisposable(m_LockAllLevelsButton.onClick.AsObservable().Subscribe(_ => viewModel.LockAllLevels()));
            
            gameObject.SetActive(true);
            m_MenuObject.SetActive(false);
        }
        
        private void OpenMenu()
        {
            m_InGameOpenButton.interactable = false;
            m_MenuObject.SetActive(true);
        }

        private void CloseMenu()
        {
            m_InGameOpenButton.interactable = true;
            m_MenuObject.SetActive(false);
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}