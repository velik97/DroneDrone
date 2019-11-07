using System.Linq;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Control
{
    public class TouchInputReader : DisposableContainer, IInputReader, ITouchInputHandler
    {
        private bool m_RightIsPressed = false;
        private bool m_LeftIsPressed = false;

        public TouchInputReader()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public bool RightIsPressed()
        {
            return m_RightIsPressed;
        }

        public bool LeftIsPressed()
        {
            return m_LeftIsPressed;
        }

        public void HandleRightTouchValueChanged(bool value)
        {
            m_RightIsPressed = value;
        }

        public void HandleLeftTouchValueChanged(bool value)
        {
            m_LeftIsPressed = value;
        }
    }
}