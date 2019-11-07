using Util.EventBusSystem;

namespace Drone.Control
{
    public interface ITouchInputHandler : IGlobalSubscriber
    {
        void HandleRightTouchValueChanged(bool value);
        void HandleLeftTouchValueChanged(bool value);
    }
}