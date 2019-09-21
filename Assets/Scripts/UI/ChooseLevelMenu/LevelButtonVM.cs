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

        public Action OnClickAction;

        public LevelButtonVM()
        {
        }

        public void OnClick()
        {
            OnClickAction?.Invoke();
        }
    }
}