using GameProcessManaging;
using UI.MVVM;
using UniRx;
using UnityEngine.UI;
using Util.EventBusSystem;

namespace UI.WinMenu
{
    public class WinMenuVM : ViewModel, IGameFinishHandler, IRestoreStateHandler
    {        
        public ReactiveCommand OnGameFinish = new ReactiveCommand();
        public ReactiveCommand OnRestore = new ReactiveCommand();
        public bool HasNextLevel => SceneNames.Instance.HasNextLevelScene();
        
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
            EventBus.TriggerEvent<IRestartGameRequestHandler>(h => h.HandleRestartGame());
        }
        
        public void GoToNextLevel()
        {
            EventBus.TriggerEvent<IGoToNextLevelHandler>(h => h.HandleGoToNextLevel());
        }

        public void GoToMainMenu()
        {
            EventBus.TriggerEvent<IGoToMenuHandler>(h => h.HandleGoToMenu());
        }

        public void HandleRestoreState()
        {
            OnRestore.Execute();
        }
    }
}