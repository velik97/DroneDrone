using UI.MVVM;
using UniRx;
using UnityEngine;

namespace UI.TouchControl
{
    public class TouchControlView : View<TouchControlVM>
    {
        [SerializeField] private TouchButton m_RightTouchButton;
        [SerializeField] private TouchButton m_LeftTouchButton;

        public override void Bind(TouchControlVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(m_RightTouchButton.IsPressed.Subscribe(viewModel.SetRightEngine));
            AddDisposable(m_LeftTouchButton.IsPressed.Subscribe(viewModel.SetLeftEngine));
            
            gameObject.SetActive(true);
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}