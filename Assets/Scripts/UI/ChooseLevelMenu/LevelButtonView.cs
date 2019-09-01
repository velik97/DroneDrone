using UI.MVVM;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ChooseLevelMenu
{
    public class LevelButtonView : View<LevelButtonVM>
    {
        [SerializeField]
        private Button m_Button;
        [SerializeField]
        private Text m_NameText;

        public override void Bind(LevelButtonVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(m_Button.onClick.AsObservable().Subscribe(_ => viewModel.OnClick()));
            
            AddDisposable(viewModel.IsPresent.Subscribe(SetPresence));
            AddDisposable(viewModel.IsAvailable.Subscribe(SetAvailable));
            AddDisposable(viewModel.LevelName.Subscribe(SetName));
        }

        private void SetPresence(bool value)
        {
            gameObject.SetActive(value);
        }

        private void SetAvailable(bool value)
        {
            m_Button.interactable = value;
        }

        private void SetName(string value)
        {
            m_NameText.text = value;
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}