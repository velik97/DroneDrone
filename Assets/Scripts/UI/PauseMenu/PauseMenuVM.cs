using GameProcessManaging;
using UI.MVVM;
using UniRx;
using UnityEngine.EventSystems;
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
            EventBus.TriggerEvent<IGoToMenuHandler>(h => h.HandleGoToMenu());
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