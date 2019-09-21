using Util.EventBusSystem;

namespace LevelsProgressManagement
{
    public interface ILastAvailableLevelChangedHandler : IGlobalSubscriber
    {
        void HandleLastAvailableLevelChanged();
    }
}