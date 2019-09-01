using System.Collections.Generic;
using LevelsProgressManagement;
using SceneLoading;
using UI.MVVM;
using UniRx;
using UnityEngine.UI;

namespace UI.ChooseLevelMenu
{
    public class ChooseLevelMenuVM : ViewModel
    {
        public const int BUTTONS_ON_SCREEN_COUNT = 15;
        public readonly LevelButtonVM[] LevelButtonVms;

        public readonly BoolReactiveProperty NextLevelsAreAvailable = new BoolReactiveProperty();
        public readonly BoolReactiveProperty PrevLevelsAreAvailable = new BoolReactiveProperty();

        private int m_FirstButtonNum;

        public ChooseLevelMenuVM()
        {
            LevelButtonVms = new LevelButtonVM[BUTTONS_ON_SCREEN_COUNT];

            for (var i = 0; i < LevelButtonVms.Length; i++)
            {
                int temp = i;
                LevelButtonVms[i] = new LevelButtonVM(() => OnLevelButtonClicked(temp));
            }

            m_FirstButtonNum = LevelsProgression.LastAvailableLevel / BUTTONS_ON_SCREEN_COUNT * BUTTONS_ON_SCREEN_COUNT;
            
            UpdateLevelButtons();
            UpdateNavigationButtons();
        }

        public void OnPrevButtonClicked()
        {
            m_FirstButtonNum -= BUTTONS_ON_SCREEN_COUNT;

            UpdateLevelButtons();
            UpdateNavigationButtons();
        }
        
        public void OnNextButtonClicked()
        {
            m_FirstButtonNum += BUTTONS_ON_SCREEN_COUNT;

            UpdateLevelButtons();
            UpdateNavigationButtons();
        }

        private void UpdateLevelButtons()
        {
            for (var i = 0; i < LevelButtonVms.Length; i++)
            {
                int num = i + m_FirstButtonNum;
                LevelButtonVms[i].LevelName.Value = (num + 1).ToString();
                LevelButtonVms[i].IsPresent.Value = num < SceneNames.Instance.GetLevelsCount();
                LevelButtonVms[i].IsAvailable.Value = num <= LevelsProgression.LastAvailableLevel;
            }
        }

        private void UpdateNavigationButtons()
        {
            PrevLevelsAreAvailable.Value = m_FirstButtonNum > 0;
            NextLevelsAreAvailable.Value = m_FirstButtonNum + BUTTONS_ON_SCREEN_COUNT <
                                           SceneNames.Instance.GetLevelsCount();
        }

        private void OnLevelButtonClicked(int buttonNum)
        {
            SceneLoader.LoadScene(SceneNames.Instance.GetLevelSceneName(buttonNum + m_FirstButtonNum));
        }
    }
}