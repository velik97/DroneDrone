using System;
using Drone.Physics;
using GameProcessManaging;
using UniRx;
using Util;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace Drone.Control
{
    public class DroneControl : DisposableContainerMonoBehaviour,
        IGamePauseHandler,
        IGameOverHandler,
        IGameFinishHandler,
        IDestroySceneHandler,
        IRestoreStateHandler
    {
        private bool m_RightWasPressed = false;
        private bool m_LeftWasPressed = false;

        private ReactiveCommand OnRightIsJustPressed = new ReactiveCommand();
        private ReactiveCommand OnLeftIsJustPressed = new ReactiveCommand();
        private ReactiveCommand OnRightIsJustReleased = new ReactiveCommand();
        private ReactiveCommand OnLeftIsJustReleased = new ReactiveCommand();

        private IInputHandler m_InputHandler;

        private DronePhysicsBase m_Physics;

        private bool m_ControlIsSuppressed = false;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            m_InputHandler = new KeyboardInputHandler();
#elif PLATFORM_IOS || PLATFORM_ANDROID
            m_InputHandler = new TouchInputHandler();
#endif

            AddDisposable(EventBus.Subscribe(this));
        }

        public void SubscribeOnControl(DronePhysicsBase physic)
        {
            m_Physics = physic;
            AddDisposable(OnRightIsJustPressed.Subscribe(_ => m_Physics.TurnOnRight()));
            AddDisposable(OnLeftIsJustPressed.Subscribe(_ => m_Physics.TurnOnLeft()));
            
            AddDisposable(OnRightIsJustReleased.Subscribe(_ => m_Physics.TurnOffRight()));
            AddDisposable(OnLeftIsJustReleased.Subscribe(_ => m_Physics.TurnOffLeft()));
        }

        private void SuppressControl()
        {
            m_Physics.TurnOffLeft();
            m_Physics.TurnOffRight();

            m_ControlIsSuppressed = true;
        }

        private void RestoreControl()
        {
            m_ControlIsSuppressed = false;
        }

        private void Update()
        {
            if (m_ControlIsSuppressed || IsDisposed)
            {
                return;
            }
            UpdateInput();
        }

        private void UpdateInput()
        {
            bool rightIsPressed = m_InputHandler?.RightIsPressed() ?? false;
            bool leftIsPressed = m_InputHandler?.LeftIsPressed() ?? false;
            
            if (rightIsPressed && !m_RightWasPressed)
            {
                OnRightIsJustPressed.Execute();
            }
            else if (!rightIsPressed && m_RightWasPressed)
            {
                OnRightIsJustReleased.Execute();
            }
            
            if (leftIsPressed && !m_LeftWasPressed)
            {
                OnLeftIsJustPressed.Execute();
            }
            else if (!leftIsPressed && m_LeftWasPressed)
            {
                OnLeftIsJustReleased.Execute();
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