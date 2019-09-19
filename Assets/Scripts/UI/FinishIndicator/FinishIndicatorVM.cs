using GameProcessManaging;
using UI.MVVM;
using UniRx;
using Util.EventBusSystem;

namespace UI.FinishIndicator
{
    public class FinishIndicatorVM : ViewModel, IGameFinishCountDownPercentageHandler, IGameFinishHandler, IRestoreStateHandler
    {
        public ReactiveCommand OnRestore = new ReactiveCommand();
        public FloatReactiveProperty FinishCountDownPercentage = new FloatReactiveProperty();
        public ReactiveCommand OnFinish = new ReactiveCommand();

        public FinishIndicatorVM()
        {
            AddDisposable(EventBus.Subscribe(this));
        }

        public void HandleGameFinishCountDownPercentageChanged(float percentage)
        {
            FinishCountDownPercentage.Value = percentage;
        }

        public void HandleGameFinish()
        {
            FinishCountDownPercentage.Value = 1f;
            OnFinish.Execute();
        }

        public void HandleRestoreState()
        {
            OnRestore.Execute();
        }
    }
}