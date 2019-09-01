using Drone.Control;
using GameProcessManaging;
using UnityEditor;

namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        public const string ASSET_ROOT_PATH = "Assets/Resources/AssetRoot.asset";
        
        private static AssetRoot s_Instance;
        public static AssetRoot Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = AssetDatabase.LoadAssetAtPath<AssetRoot>(ASSET_ROOT_PATH);
                }
                return s_Instance;
            }
        }

        public LevelScenesList LevelScenesList;
        
        public GameOverAndFinishSettings GameOverAndFinishSettings;
        
        public KeyboardControlSettings KeyboardControlSettings;
        
        public StringRoot StringRoot;
    }
}