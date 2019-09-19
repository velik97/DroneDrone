using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WinMenu
{
    public class WinMenuView : View<WinMenuVM>
    {
        [SerializeField]
        private Button m_GoToNextLevel;
        [SerializeField]
        private Button m_TryAgain;
        [SerializeField]
        private Button m_GoToMainMenuButton;

        public override void Bind(WinMenuVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(m_TryAgain.onClick.AsObservable().Subscribe(_ => viewModel.TryAgain()));
            AddDisposable(m_GoToNextLevel.onClick.AsObservable().Subscribe(_ => viewModel.GoToNextLevel()));
            AddDisposable(m_GoToMainMenuButton.onClick.AsObservable().Subscribe(_ => viewModel.GoToMainMenu()));
            
            AddDisposable(viewModel.OnGameFinish.Subscribe(_ => Open()));
            AddDisposable(viewModel.OnRestore.Subscribe(_ => Close()));
            
            m_GoToNextLevel.gameObject.SetActive(viewModel.HasNextLevel);
            
            Close();
        }

        private void Open()
        {
            gameObject.SetActive(true);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
        
        protected override void DestroyViewImplementation()
        {
        }
    }
}