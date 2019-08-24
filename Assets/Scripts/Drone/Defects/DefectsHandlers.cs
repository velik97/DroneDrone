using Util.EventBusSystem;

namespace Drone.Defects
{
    public interface IDroneBadSignalZoneHandler : IGlobalSubscriber
    {
        void HandleEnteredBadSignalZone();
        void HandleEscapedBadSignalZone();
    }

    public interface IDroneBladesDamageHandler : IGlobalSubscriber
    {
        void HandleOneEngineIsBroken();
        void HandleBothEnginesAreBroken();
    }
}