using Drone.Defects;
using GameProcessManaging;
using UI.MVVM;
using UniRx;
using Util.EventBusSystem;

namespace DefaultNamespace
{
    public class QuickRestartVM : ViewModel, IDroneBladesDamageHandler, IGameOverCountDownHandler, IGameFinishCountDownHandler, IRestoreStateHandler
    {
        public ReactiveCommand OnOpen = new ReactiveCommand();
        public ReactiveCommand OnClose = new ReactiveCommand();
        
        private bool m_EngineIsBroken = false;

        public QuickRestartVM()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public void RestartGame()
        {
            EventBus.TriggerEvent<IRestartGameRequestHandler>(h => h.HandleRestartGame());
        }

        public void HandleOneEngineIsBroken()
        {
            m_EngineIsBroken = true;
            OnOpen.Execute();
        }

        public void HandleBothEnginesAreBroken()
        {
        }

        public void HandleStartGameFinishCountDown()
        {
            OnClose.Execute();
        }

        public void HandleInterruptGameFinishCountDown()
        {
            if (m_EngineIsBroken)
            {
                OnOpen.Execute();
            }
        }

        public void HandleCompleteGameFinishCountDown()
        {
            OnClose.Execute();
        }

        public void HandleStartGameOverCountDown()
        {
            OnOpen.Execute();
        }

        public void HandleInterruptGameOverCountDown()
        {
            if (!m_EngineIsBroken)
            {
                OnClose.Execute();
            }
        }

        public void HandleCompleteGameOverCountDown()
        {
            OnClose.Execute();
        }

        public void HandleRestoreState()
        {
            OnClose.Execute();
        }
    }
}