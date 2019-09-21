using System.Collections.Generic;
using GameProcessManaging;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Util.EventBusSystem;

namespace LevelsProgressManagement
{
    public static class LevelsProgression
    {
        private const string LAST_AVAILABLE_LEVEL_KEY = "Last Available Level";

        private static int LevelsCount => SceneNames.Instance.GetLevelsCount();

        private static int s_LastAvailableLevel = -1;
        
        public static int LastAvailableLevel
        {
            get
            {
                if (s_LastAvailableLevel == -1)
                {
                    s_LastAvailableLevel = PlayerPrefs.HasKey(LAST_AVAILABLE_LEVEL_KEY)
                        ? PlayerPrefs.GetInt(LAST_AVAILABLE_LEVEL_KEY)
                        : 0;
                }
                return s_LastAvailableLevel;
            }
            private set
            {
                PlayerPrefs.SetInt(LAST_AVAILABLE_LEVEL_KEY, value);
                s_LastAvailableLevel = value;
                EventBus.TriggerEvent<ILastAvailableLevelChangedHandler>(h => h.HandleLastAvailableLevelChanged());
            }
        }

        public static void SetNextLevelAvailable()
        {
            if (LastAvailableLevel < LevelsCount - 1)
            {
                SendLevelAnalyticsData();
                LastAvailableLevel += 1;
            }
        }
        
        public static void ForceUnlockAllLevels()
        {
            LastAvailableLevel = LevelsCount - 1;
        }

        public static void ForceLockAllLevels()
        {
            LastAvailableLevel = 0;
        }
        
        private static void SendLevelAnalyticsData()
        {
            Dictionary<string, object> analyticsData = new Dictionary<string, object>();
            
            analyticsData["Avg Try Time"] = GameStateManager.AvgTryTime;
            analyticsData["Complete Time"] = GameStateManager.CompleteTime;
            analyticsData["Total Time"] = GameStateManager.TotalTime;
            analyticsData["Restarts Count"] = GameStateManager.RestartsCount;
            
            AnalyticsEvent.LevelComplete(LastAvailableLevel, analyticsData);
        }
    }
}