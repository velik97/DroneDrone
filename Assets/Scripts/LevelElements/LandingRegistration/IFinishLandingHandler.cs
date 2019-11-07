using Util.EventBusSystem;

namespace LevelElements.LandingRegistration
{
    public interface IFinishLandingHandler : IGlobalSubscriber
    {
        void HandleStartedFinishLanding();
        void HandleInterruptedFinishLanding();
    }
}