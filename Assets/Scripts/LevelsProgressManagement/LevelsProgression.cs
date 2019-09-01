using UnityEngine;
using UnityEngine.UI;

namespace LevelsProgressManagement
{
    public static class LevelsProgression
    {
        private const string LAST_AVAILABLE_LEVEL_KEY = "Last Available Level";

        private static int LevelsCount => SceneNames.Instance.GetLevelsCount();
        
        public static int LastAvailableLevel
        {
            get => PlayerPrefs.HasKey(LAST_AVAILABLE_LEVEL_KEY) ? PlayerPrefs.GetInt(LAST_AVAILABLE_LEVEL_KEY) : 0;
            private set => PlayerPrefs.SetInt(LAST_AVAILABLE_LEVEL_KEY, value);
        }

        public static void SetNextLevelAvailable()
        {
            if (LastAvailableLevel < LevelsCount - 1)
            {
                LastAvailableLevel += 1;
            }
        }
    }
}