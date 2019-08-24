using Util.EventBusSystem;

namespace LevelProgression
{
    public interface IFinishLandingHandler : IGlobalSubscriber
    {
        void HandleStartedFinishLanding();
        void HandleInterruptedFinishLanding();
    }
}