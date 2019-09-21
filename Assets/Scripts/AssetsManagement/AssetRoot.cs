using Drone.Control;
using GameProcessManaging;

namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        public const string ASSET_ROOT_PATH = "AssetRoot";
        
        private static AssetRoot s_Instance;
        public static AssetRoot Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = Resources.Load<AssetRoot>(ASSET_ROOT_PATH);
                }
                return s_Instance;
            }
        }

        public SceneNames m_SceneNames;
        
        public GameOverAndFinishSettings GameOverAndFinishSettings;
        
        public KeyboardControlSettings KeyboardControlSettings;
        
        public StringRoot StringRoot;
    }
}