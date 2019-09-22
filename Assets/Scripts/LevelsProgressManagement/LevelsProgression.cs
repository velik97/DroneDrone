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
        private const string LAST_DONE_LEVEL_KEY = "Last Done Level";

        private static int LevelsCount => SceneNames.Instance.GetLevelsCount();

        private static int s_LastDoneLevel = int.MinValue;

        public static int LastAvailableLevel => LastDoneLevel + 1 == LevelsCount ? LastDoneLevel : LastDoneLevel + 1;
        
        private static int LastDoneLevel
        {
            get
            {
                if (s_LastDoneLevel == int.MinValue)
                {
                    s_LastDoneLevel = PlayerPrefs.HasKey(LAST_DONE_LEVEL_KEY)
                        ? PlayerPrefs.GetInt(LAST_DONE_LEVEL_KEY)
                        : -1;
                }
                return s_LastDoneLevel;
            }
            set
            {
                PlayerPrefs.SetInt(LAST_DONE_LEVEL_KEY, value);
                s_LastDoneLevel = value;
                EventBus.TriggerEvent<ILastAvailableLevelChangedHandler>(h => h.HandleLastAvailableLevelChanged());
            }
        }

        public static void SetNextLevelAvailable()
        {
            if (LastDoneLevel < LevelsCount - 1)
            {
                SendLevelAnalyticsData();
                LastDoneLevel++;
            }
        }
        
        public static void ForceUnlockAllLevels()
        {
            LastDoneLevel = LevelsCount - 1;
        }

        public static void ForceLockAllLevels()
        {
            LastDoneLevel = -1;
        }
        
        private static void SendLevelAnalyticsData()
        {
            Dictionary<string, object> analyticsData = new Dictionary<string, object>();
            
            analyticsData["Avg Try Time"] = GameStateManager.AvgTryTime;
            analyticsData["Complete Time"] = GameStateManager.CompleteTime;
            analyticsData["Total Time"] = GameStateManager.TotalTime;
            analyticsData["Restarts Count"] = GameStateManager.RestartsCount;
            
            AnalyticsEvent.LevelComplete(LastDoneLevel, analyticsData);
        }
    }
}