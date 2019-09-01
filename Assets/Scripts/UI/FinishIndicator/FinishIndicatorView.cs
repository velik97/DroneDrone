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

        private float m_Aim = 0f;
        private float m_CurrentValue = 0f;

        public override void Bind(FinishIndicatorVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(viewModel.FinishCountDownPercentage.Subscribe(SetPercentage));
            AddDisposable(viewModel.OnFinish.Subscribe(_ => SetFinishColor()));
            
            SetUsualColor();
        }
        
        private void Update()
        {
            m_CurrentValue = Mathf.Lerp(m_CurrentValue, m_Aim, Time.deltaTime * m_Speed);

            m_BarImage.fillAmount = m_CurrentValue;
        }
        
        private void SetPercentage(float value)
        {
            m_Aim = value;
        } 
        
        private void SetUsualColor()
        {
            SetColor(m_UsualColor);
        }

        private void SetFinishColor()
        {
            SetColor(m_FinishColor);
        }

        private void SetColor(Color c)
        {
            m_BarImage.color = c;
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}