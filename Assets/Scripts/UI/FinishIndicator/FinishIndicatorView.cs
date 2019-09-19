using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FinishIndicator
{
    public class FinishIndicatorView : View<FinishIndicatorVM>
    {
        [SerializeField]
        private Image m_BarImage;
        [SerializeField]
        private float m_Speed;
        
        [SerializeField]
        private Color m_UsualColor;
        [SerializeField]
        private Color m_FinishColor;

        private float m_AimValue = 0f;
        private float m_CurrentValue = 0f;

        public override void Bind(FinishIndicatorVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(viewModel.OnRestore.Subscribe(_ => SetForcePercentage(0f)));
            AddDisposable(viewModel.FinishCountDownPercentage.Subscribe(SetPercentage));
        }
        
        private void SetPercentage(float value)
        {
            m_AimValue = value;
        }
        
        private void SetForcePercentage(float value)
        {
            m_AimValue = value;
            m_CurrentValue = value;
        }
        
        private void Update()
        {
            m_CurrentValue = Mathf.Lerp(m_CurrentValue, m_AimValue, Time.deltaTime * m_Speed);

            m_BarImage.fillAmount = m_CurrentValue;
            m_BarImage.color = Color.Lerp(m_UsualColor, m_FinishColor, m_CurrentValue);
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}