using Util.EventBusSystem;

namespace LandingRegistration
{
    public interface IFinishLandingHandler : IGlobalSubscriber
    {
        void HandleStartedFinishLanding();
        void HandleInterruptedFinishLanding();
    }
}