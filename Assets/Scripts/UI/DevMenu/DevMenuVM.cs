using GameProcessManaging;
using LevelsProgressManagement;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.DevMenu
{
    public class DevMenuVM : ViewModel
    {
        public void PauseGame()
        {
            EventBus.TriggerEvent<IGamePauseHandler>(h => h.HandlePause());
        }

        public void UnPauseGame()
        {
            EventBus.TriggerEvent<IGamePauseHandler>(h => h.HandleUnPause());
        }
        
        public void UnlockAllLevels()
        {
            LevelsProgression.ForceUnlockAllLevels();
        }

        public void LockAllLevels()
        {
            LevelsProgression.ForceLockAllLevels();
        }
    }
}