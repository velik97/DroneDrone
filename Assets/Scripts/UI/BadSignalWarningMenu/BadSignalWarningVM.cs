using Drone.Defects;
using GameProcessManaging;
using UI.MVVM;
using UniRx;
using UnityEngine.UI;
using Util.EventBusSystem;

namespace UI.BadSignalWarningMenu
{
    public class BadSignalWarningVM : ViewModel, IDroneBadSignalZoneHandler, IGameOverCountDownPercentageHandler, IRestoreStateHandler
    {
        public ReactiveCommand OnRestore = new ReactiveCommand();
        public FloatReactiveProperty BadSignalPercentage = new FloatReactiveProperty();
        public string WarningText => StringRoot.Instance.BadSignalWarning;

        private bool m_IsInBadSignalZone = false;

        public BadSignalWarningVM()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public void HandleEnteredBadSignalZone()
        {
            m_IsInBadSignalZone = true;
        }

        public void HandleEscapedBadSignalZone()
        {
            m_IsInBadSignalZone = false;
        }

        public void HandleGameOverCountDownPercentageChanged(float percentage)
        {
            if (percentage == 0f || m_IsInBadSignalZone)
            {
                BadSignalPercentage.Value = percentage;
            }
        }

        public void HandleRestoreState()
        {
            HandleEscapedBadSignalZone();
            OnRestore.Execute();
        }
    }
}