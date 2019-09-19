using GameProcessManaging;
using UI.MVVM;
using UniRx;
using Util.EventBusSystem;

namespace UI.PauseMenu
{
    public class PauseMenuVM : ViewModel, IGameOverHandler, IGameFinishHandler, IRestoreStateHandler
    {
        public ReactiveCommand OnGameFinishedOrOver = new ReactiveCommand();
        public ReactiveCommand OnRestore = new ReactiveCommand();
        
        public PauseMenuVM()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public void PauseGame()
        {
            EventBus.TriggerEvent<IGamePauseHandler>(h => h.HandlePause());
        }

        public void UnPauseGame()
        {
            EventBus.TriggerEvent<IGamePauseHandler>(h => h.HandleUnPause());
        }

        public void RestartGame()
        {
            EventBus.TriggerEvent<IRestartGameRequestHandler>(h => h.HandleRestartGame());
        }

        public void GoToMainMenu()
        {
            EventBus.TriggerEvent<IGoToMenuHandler>(h => h.HandleGoToMenu());
        }

        public void HandleGameOver()
        {
            OnGameFinishedOrOver.Execute();
        }

        public void HandleGameFinish()
        {
            OnGameFinishedOrOver.Execute();
        }

        public void HandleRestoreState()
        {
            OnRestore.Execute();
        }
    }
}