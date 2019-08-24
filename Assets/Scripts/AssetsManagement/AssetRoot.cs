using System.Linq;
using Drone.Control;
using GameProcessManaging;

namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/Asset Root")]
    public class AssetRoot : ScriptableObject
    {
        private static AssetRoot s_Instance;

        public static AssetRoot Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    Debug.Log("Asset Root is null");
                    s_Instance = Resources.FindObjectsOfTypeAll<AssetRoot>().FirstOrDefault();
                    if (s_Instance == null)
                    {
                        Debug.Log("Asset Root is still null(((");
                    }
                }
                return s_Instance;
            }
        }

        public GameOverAndFinishSettings GameOverAndFinishSettings;
        public KeyboardControlSettings KeyboardControlSettings;
    }
}