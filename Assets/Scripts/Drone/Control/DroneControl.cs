using System;
using Drone.Physics;
using GameProcessManaging;
using UniRx;
using Util;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace Drone.Control
{
    public class DroneControl : DisposableContainer,
        IGamePauseHandler,
        IGameOverHandler,
        IGameFinishHandler,
        IDestroySceneHandler,
        IRestoreStateHandler
    {
        private bool m_RightWasPressed = false;
        private bool m_LeftWasPressed = false;

        private readonly BoolReactiveProperty m_RightIsPressed = new BoolReactiveProperty();
        private readonly BoolReactiveProperty m_LeftIsPressed = new BoolReactiveProperty();

        public IReadOnlyReactiveProperty<bool> RightIsPressed => m_RightIsPressed;
        public IReadOnlyReactiveProperty<bool> LeftIsPressed => m_LeftIsPressed;

        private IInputReader m_InputReader;
        
        private bool m_ControlIsSuppressed = false;

        public DroneControl()
        {
#if UNITY_EDITOR
            m_InputReader = new KeyboardInputReader();
#elif PLATFORM_IOS || PLATFORM_ANDROID
            m_InputReader = new TouchInputReader();
            AddDisposable(m_InputReader as IDisposable);
#endif

            AddDisposable(EventBus.Subscribe(this));
        }

        private void SuppressControl()
        {
            m_RightIsPressed.Value = false;
            m_LeftIsPressed.Value = false;

            m_ControlIsSuppressed = true;
        }

        private void RestoreControl()
        {
            m_ControlIsSuppressed = false;
        }

        public void Update()
        {
            if (m_ControlIsSuppressed || IsDisposed)
            {
                return;
            }
            UpdateInput();
        }

        private void UpdateInput()
        {
            bool rightIsPressed = m_InputReader?.RightIsPressed() ?? false;
            bool leftIsPressed = m_InputReader?.LeftIsPressed() ?? false;
            
            if (rightIsPressed && !m_RightWasPressed)
            {
                m_RightIsPressed.Value = true;
            }
            else if (!rightIsPressed && m_RightWasPressed)
            {
                m_RightIsPressed.Value = false;
            }
            
            if (leftIsPressed && !m_LeftWasPressed)
            {
                m_LeftIsPressed.Value = true;
            }
            else if (!leftIsPressed && m_LeftWasPressed)
            {
                m_LeftIsPressed.Value = false;
            }

            m_RightWasPressed = rightIsPressed;
            m_LeftWasPressed = leftIsPressed;
        }
        
        public void HandleGameOver()
        {
            SuppressControl();
        }

        public void HandleGameFinish()
        {
            SuppressControl();
        }
        
        public void HandlePause()
        {
            SuppressControl();
        }

        public void HandleUnPause()
        {
            RestoreControl();
        }

        public void HandleRestoreState()
        {
            RestoreControl();
        }
        
        public void HandleDestroyScene()
        {
            Dispose();
        }
    }
}