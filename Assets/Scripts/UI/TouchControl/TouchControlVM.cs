using Drone.Control;
using UI.MVVM;
using Util.EventBusSystem;

namespace UI.TouchControl
{
    public class TouchControlVM : ViewModel
    {
        public void SetRightEngine(bool value)
        {
            EventBus.TriggerEvent<ITouchInputHandler>(h => h.HandleRightTouchValueChanged(value));
        }
        
        public void SetLeftEngine(bool value)
        {
            EventBus.TriggerEvent<ITouchInputHandler>(h => h.HandleLeftTouchValueChanged(value));
        }
    }
}