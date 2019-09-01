using Util.EventBusSystem;

namespace Util.GlobalInitializationSystem
{
    public interface IDestroySceneHandler : IGlobalSubscriber
    {
        void HandleDestroyScene();
    }
}