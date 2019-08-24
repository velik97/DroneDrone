using GameProcessManaging;
using UI.MVVM;
using UniRx;
using Util.EventBusSystem;

namespace UI.PauseMenu
{
    public class PauseMenuVM : ViewModel, IGameOverHandler, IGameFinishHandler
    {
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
            EventBus.TriggerEvent<IRestartGameHandler>(h => h.HandleRestartGame());
        }

        public void GoToMainMenu()
        {
            
        }

        public void HandleGameOver()
        {
            Dispose();
        }

        public void HandleGameFinish()
        {
            Dispose();
        }
    }
}