using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Drone.Control
{
    [CreateAssetMenu(menuName = "Assets/UI Settings")]
    public class KeyboardControlSettings : ScriptableObject
    {
        public static KeyboardControlSettings Instance => AssetRoot.Instance.KeyboardControlSettings;
        
        [SerializeField]
        private KeyCode[] m_RightEngineKeys;
        [SerializeField]
        private KeyCode[] m_LeftEngineKeys;

        public KeyCode[] RightEngineKeys => m_RightEngineKeys;
        public KeyCode[] LeftEngineKeys => m_LeftEngineKeys;
    }
}