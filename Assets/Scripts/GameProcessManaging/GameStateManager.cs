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
    public class GameStateManager : BaseDisposable,
        IGlobalInitializableInGame,
        IRestartGameHandler,
        IGamePauseHandler,
        IGoToNextLevelHandler,
        IGoToMenuHandler,
        IGameFinishHandler
    {        
        public GameStateManager()
        {
        }

        public InitializePrior InitializePrior => InitializePrior.UsualAwake;

        public void Initialize()
        {
            AddDisposable(EventBus.Subscribe(this));
        }
        
        public void HandleRestartGame()
        {
            HandleUnPause();
            SceneLoader.LoadCurrentScene();
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