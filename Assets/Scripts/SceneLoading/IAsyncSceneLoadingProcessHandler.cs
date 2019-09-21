using Util.EventBusSystem;

namespace SceneLoading
{
    public interface IAsyncSceneLoadingProcessHandler : IGlobalSubscriber
    {
        void HandleAsyncSceneLoadingProcess(float value);
    }
}