using System;
using UI.MVVM;
using UniRx;

namespace UI.ChooseLevelMenu
{
    public class LevelButtonVM : ViewModel
    {
        public readonly BoolReactiveProperty IsPresent = new BoolReactiveProperty();
        public readonly BoolReactiveProperty IsAvailable = new BoolReactiveProperty();

        public readonly StringReactiveProperty LevelName = new StringReactiveProperty();

        private Action m_OnClickAction;

        public LevelButtonVM(Action onClickAction)
        {
            m_OnClickAction = onClickAction;
        }

        public void OnClick()
        {
            m_OnClickAction?.Invoke();
        }
    }
}