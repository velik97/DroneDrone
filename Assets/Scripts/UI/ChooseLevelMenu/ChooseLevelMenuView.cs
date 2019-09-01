using UI.MVVM;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


namespace UI.ChooseLevelMenu
{
    public class ChooseLevelMenuView : View<ChooseLevelMenuVM>
    {
        [SerializeField]
        private Button m_NextLevelsButton;
        [SerializeField]
        private Button m_PrevLevelsButton;

        [SerializeField]
        private LevelButtonView m_LevelButtonPrefab;
        [SerializeField]
        private RectTransform m_LevelButtonsContainer;

        public override void Bind(ChooseLevelMenuVM viewModel)
        {
            base.Bind(viewModel);
            
            AddDisposable(viewModel.NextLevelsAreAvailable.Subscribe(SetNextButtonActive));
            AddDisposable(viewModel.PrevLevelsAreAvailable.Subscribe(SetPrevButtonActive));
            
            AddDisposable(m_NextLevelsButton.onClick.AsObservable().Subscribe(_ => viewModel.OnNextButtonClicked()));
            AddDisposable(m_PrevLevelsButton.onClick.AsObservable().Subscribe(_ => viewModel.OnPrevButtonClicked()));

            for (int i = 0; i < ChooseLevelMenuVM.BUTTONS_ON_SCREEN_COUNT; i++)
            {
                LevelButtonView newLevelButton = Instantiate(m_LevelButtonPrefab, m_LevelButtonsContainer);
                newLevelButton.transform.SetAsLastSibling();
                newLevelButton.Bind(viewModel.LevelButtonVms[i]);
            }
        }

        private void SetNextButtonActive(bool value)
        {
            m_NextLevelsButton.interactable = value;
        }
        
        private void SetPrevButtonActive(bool value)
        {
            m_PrevLevelsButton.interactable = value;
        }

        protected override void DestroyViewImplementation()
        {
        }
    }
}