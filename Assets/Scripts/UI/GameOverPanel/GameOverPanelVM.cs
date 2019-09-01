using GameProcessManaging;
using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Util.EventBusSystem;

namespace UI.GameOverPanel
{
    public class GameOverPanelVM : ViewModel, IGameOverCountDownHandler, IGameOverCountDownPercentageHandler
    {       
        public StringReactiveProperty GameOverCountDownText = new StringReactiveProperty();
        public ReactiveCommand OnOpen = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();
        public ReactiveCommand OnGameOver = new ReactiveCommand();

        private readonly int m_TotalCountDownSeconds;
        private int m_LastSecondsShown = -1;

        public GameOverPanelVM()
        {
            m_TotalCountDownSeconds = AssetRoot.Instance.GameOverAndFinishSettings.GameOverCountDown;
            
            AddDisposable(EventBus.Subscribe(this));
        }

        public void RestartGame()
        {
            EventBus.TriggerEvent<IRestartGameHandler>(h => h.HandleRestartGame());
        }

        public void GoToMainMenu()
        {
            EventBus.TriggerEvent<IGoToMenuHandler>(h => h.HandleGoToMenu());
        }

        public void HandleStartGameOverCountDown()
        {
            OnOpen.Execute();
        }

        public void HandleInterruptGameOverCountDown()
        {
            OnClose.Execute();
            m_LastSecondsShown = -1;
        }

        public void HandleCompleteGameOverCountDown()
        {
            OnGameOver.Execute();
            GameOverCountDownText.Value = StringRoot.Instance.GameOverText;
        }

        public void HandleGameOverCountDownPercentageChanged(float percentage)
        {
            int seconds = Mathf.CeilToInt((1f - percentage) * m_TotalCountDownSeconds);
            if (m_LastSecondsShown == seconds)
            {
                return;
            }
            
            m_LastSecondsShown = seconds;
            
            GameOverCountDownText.Value = m_LastSecondsShown != 0
                ? string.Format(StringRoot.Instance.GameOverCountDownText, m_LastSecondsShown)
                : StringRoot.Instance.GameOverText;
        }
    }
}