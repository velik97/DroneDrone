using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScreenPanel
{
    public class LoadingScreenView : View<LoadingScreenVM>
    {
        [SerializeField]
        private Image m_LoadingBarImage;

        public override void Bind(LoadingScreenVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(viewModel.LoadingProcess.Subscribe(SetLoadingProcess));
            AddDisposable(viewModel.OnOpen.Subscribe(_ => Open()));
            AddDisposable(viewModel.OnClose.Subscribe(_ => Close()));

            if (viewModel.IsOpened)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        private void Open()
        {
            gameObject.SetActive(true);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void SetLoadingProcess(float value)
        {
            m_LoadingBarImage.fillAmount = value;
        }
        
        protected override void DestroyViewImplementation()
        {
        }
    }
}