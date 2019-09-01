using GameProcessManaging;
using UI.MVVM;
using UniRx;
using UnityEngine.UI;
using Util.EventBusSystem;

namespace UI.WinMenu
{
    public class WinMenuVM : ViewModel, IGameFinishHandler
    {        
        public ReactiveCommand OnGameFinish = new ReactiveCommand();
        public bool HasNextLevel => LevelScenesList.Instance.HasNextScene();
        
        public WinMenuVM()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public void HandleGameFinish()
        {
            OnGameFinish.Execute();
        }
        
        public void TryAgain()
        {
            EventBus.TriggerEvent<IRestartGameHandler>(h => h.HandleRestartGame());
        }
        
        public void GoToNextLevel()
        {
            EventBus.TriggerEvent<IGoToNextLevelHandler>(h => h.HandleGoToNextLevel());
        }

        public void GoToMainMenu()
        {
            EventBus.TriggerEvent<IGoToMenuHandler>(h => h.HandleGoToMenu());
        }
    }
}