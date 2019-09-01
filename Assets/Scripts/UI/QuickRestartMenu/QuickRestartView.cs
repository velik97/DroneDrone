using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QuickRestartView : View<QuickRestartVM>
    {
        [SerializeField]
        private Button m_RestartButton;    
        
        public override void Bind(QuickRestartVM viewModel)
        {
            base.Bind(viewModel);

            AddDisposable(m_RestartButton.onClick.AsObservable().Subscribe(_ => viewModel.RestartGame()));

            AddDisposable(viewModel.OnOpen.Subscribe(_ => Open()));
            AddDisposable(viewModel.OnClose.Subscribe(_ => Close()));
            
            Close();
        }

        private void Open()
        {
            gameObject.SetActive(true);   
        }
        
        private void Close()
        {
            gameObject.SetActive(false);   
        }

        protected override void DestroyViewImplementation()
        {
            gameObject.SetActive(false);
        }
    }
}