using Util;
using Util.GlobalInitializationSystem;

namespace UnityEngine.UI
{
    public class MainMenuContext : BaseDisposable, IGlobalInitializableInMainMenu
    {
        public InitializePrior InitializePrior => InitializePrior.UsualStart;
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}