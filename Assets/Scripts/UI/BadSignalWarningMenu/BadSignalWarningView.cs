using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BadSignalWarningMenu
{
    public class BadSignalWarningView : View<BadSignalWarningVM>
    {
        [SerializeField]
        private Image m_BadSignalImage;
        [SerializeField]
        private Text m_BadSignalText;

        [SerializeField]
        private GameObject m_BadSignalImageObject;
        [SerializeField]
        private GameObject m_BadSignalTextObject;

        [SerializeField]
        private float m_Speed;
        
        private float m_AimVisibility = 0f;
        private float m_CurrentVisibility = 0f;

        public override void Bind(BadSignalWarningVM viewModel)
        {
            base.Bind(viewModel);
            
            m_BadSignalImageObject.SetActive(true);
            m_BadSignalTextObject.SetActive(true);
            
            AddDisposable(viewModel.BadSignalPercentage.Subscribe(SetVisibility));
            AddDisposable(viewModel.OnRestore.Subscribe(_ => SetForceVisibility(0f)));
            m_BadSignalText.text = viewModel.WarningText;
        }

        private void SetVisibility(float value)
        {
            m_AimVisibility = value;
        }

        private void SetForceVisibility(float value)
        {
            m_AimVisibility = value;
            m_CurrentVisibility = value;
        }

        private void Update()
        {
            m_CurrentVisibility = Mathf.Lerp(m_CurrentVisibility, m_AimVisibility, Time.deltaTime * m_Speed);
            m_BadSignalImage.color = new Color(1f, 1f, 1f, m_CurrentVisibility);
            m_BadSignalText.color = new Color(1f, 1f, 1f, 
                (m_AimVisibility == 0f ? 0f : 1f) * Mathf.Sin(Time.time*10f));
        }
        
        protected override void DestroyViewImplementation()
        {
        }
    }
}