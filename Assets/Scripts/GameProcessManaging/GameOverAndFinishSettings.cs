using UnityEngine;
using UnityEngine.UI;

namespace GameProcessManaging
{
    [CreateAssetMenu(menuName = "Assets/Game Over And Finish Settings")]
    public class GameOverAndFinishSettings : ScriptableObject
    {
        public static GameOverAndFinishSettings Instance => AssetRoot.Instance.GameOverAndFinishSettings;
        
        [SerializeField]
        private int m_GameOverCountDown;
        [SerializeField]
        private int m_GameFinishCountDown;

        public int GameOverCountDown => m_GameOverCountDown;
        public int GameFinishCountDown => m_GameFinishCountDown;
    }
}