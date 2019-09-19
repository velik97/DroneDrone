using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOverPanel
{
    public class GameOverPanelView : View<GameOverPanelVM>
    {
        [SerializeField]
        private Text m_GameOverText;

        [SerializeField]
        private GameObject m_GameOverButtonsObject;

        [SerializeField]
        private Button m_RestartButton;
        [SerializeField]
        private Button m_GoToMenuButton;

        public override void Bind(GameOverPanelVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(viewModel.OnOpen.Subscribe(_ => Open()));
            AddDisposable(viewModel.OnClose.Subscribe(_ => Close()));
            AddDisposable(viewModel.GameOverCountDownText.Subscribe(SetGameOverText));
            AddDisposable(viewModel.OnGameOver.Subscribe(_ => ShowGameOver()));
            
            AddDisposable(m_RestartButton.onClick.AsObservable().Subscribe(_ => viewModel.RestartGame()));
            AddDisposable(m_GoToMenuButton.onClick.AsObservable().Subscribe(_ => viewModel.GoToMainMenu()));
            
            Close();
            m_GameOverButtonsObject.SetActive(false);
        }

        private void Open()
        {
            gameObject.SetActive(true);
        }

        private void Close()
        {
            gameObject.SetActive(false);
            m_GameOverButtonsObject.SetActive(false);
        }

        private void SetGameOverText(string text)
        {
            m_GameOverText.text = text;
        }

        private void ShowGameOver()
        {
            m_GameOverButtonsObject.SetActive(true);
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}