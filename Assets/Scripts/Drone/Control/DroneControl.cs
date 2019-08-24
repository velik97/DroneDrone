using System;
using Drone.Physics;
using UnityEngine;

namespace Drone.Control
{
    public class DroneControl : MonoBehaviour
    {
        private bool m_RightWasPressed = false;
        private bool m_LeftWasPressed = false;

        private event Action OnRightIsJustPressed;
        private event Action OnLeftIsJustPressed;
        private event Action OnRightIsJustReleased;
        private event Action OnLeftIsJustReleased;

        private IInputHandler m_InputHandler;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            m_InputHandler = new KeyboardInputHandler();
#elif PLATFORM_IOS
            m_InputHandler = new TouchInputHandler();
#endif
        }

        public void SubscribeOnControl(DronePhysicsBase physic)
        {
            OnRightIsJustPressed += physic.TurnOnRight;
            OnLeftIsJustPressed += physic.TurnOnLeft;
            
            OnRightIsJustReleased += physic.TurnOffRight;
            OnLeftIsJustReleased += physic.TurnOffLeft;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            bool rightIsPressed = m_InputHandler?.RightIsPressed() ?? false;
            bool leftIsPressed = m_InputHandler?.LeftIsPressed() ?? false;
            
            if (rightIsPressed && !m_RightWasPressed)
            {
                OnRightIsJustPressed?.Invoke();
            }
            else if (!rightIsPressed && m_RightWasPressed)
            {
                OnRightIsJustReleased?.Invoke();
            }
            
            if (leftIsPressed && !m_LeftWasPressed)
            {
                OnLeftIsJustPressed?.Invoke();
            }
            else if (!leftIsPressed && m_LeftWasPressed)
            {
                OnLeftIsJustReleased?.Invoke();
            }

            m_RightWasPressed = rightIsPressed;
            m_LeftWasPressed = leftIsPressed;
        }
    }
}