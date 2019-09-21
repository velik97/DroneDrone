using System;
using LevelsProgressManagement;
using SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace GameProcessManaging
{
    public class GameStateManager : DisposableContainer,
        IGlobalInitializableInGame,
        IRestartGameRequestHandler,
        IGamePauseHandler,
        IGoToNextLevelHandler,
        IGoToMenuHandler,
        IGameFinishHandler
    {        
        public GameStateManager()
        {
        }

        private static int s_RestartsCount;
        private static float s_StartLevelTime;
        private static float s_StartTryTime;

        public static int RestartsCount => s_RestartsCount;
        public static float AvgTryTime => (Time.time - s_StartLevelTime) / (s_RestartsCount + 1);
        public static float TotalTime => Time.time - s_StartLevelTime;
        public static float CompleteTime => Time.time - s_StartTryTime;

        public InitializePrior InitializePrior => InitializePrior.UsualAwake;

        public void Initialize()
        {
            AddDisposable(EventBus.Subscribe(this));
            
            s_StartLevelTime = Time.time;
            s_StartTryTime = Time.time;
            s_RestartsCount = 0;
        }
        
        public void HandleRestartGame()
        {
            s_StartTryTime = Time.time;
            s_RestartsCount++;
            
            HandleUnPause();
            EventBus.TriggerEvent<IRestoreStateHandler>(h => h.HandleRestoreState());
        }

        public void HandlePause()
        {
            Time.timeScale = 0f;
        }

        public void HandleUnPause()
        {
            Time.timeScale = 1f;
        }
        
        public void HandleGoToMenu()
        {
            HandleUnPause();
            SceneLoader.LoadScene(SceneNames.Instance.GetMainMenuSceneName());
        }
        
        public void HandleGoToNextLevel()
        {
            HandleUnPause();
            SceneLoader.LoadScene(SceneNames.Instance.GetNextLevelSceneName());
        }

        public void HandleGameFinish()
        {
            if (SceneNames.Instance.GetCurrentSceneIndex() == LevelsProgression.LastAvailableLevel)
            {
                LevelsProgression.SetNextLevelAvailable();
            }
        }
    }
}