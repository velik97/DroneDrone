using UnityEngine;
using UnityEngine.UI;

namespace GameProcessManaging
{
    [CreateAssetMenu(menuName = "Assets/Game Over And Finish Settings")]
    public class GameOverAndFinishSettings : ScriptableObject
    {
        public static GameOverAndFinishSettings Instance => AssetRoot.Instance.GameOverAndFinishSettings;
        
        [SerializeField]
        private float m_GameOverCountDown;
        [SerializeField]
        private float m_GameFinishCountDown;

        public float GameOverCountDown => m_GameOverCountDown;
        public float GameFinishCountDown => m_GameFinishCountDown;
    }
}