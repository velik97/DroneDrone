using System;
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
        IGoToNextLevelHandler,
        IGamePauseHandler,
        IGoToMenuHandler
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
            // Todo исправить на норм логику, пока переносит на первый уровень
            SceneLoader.LoadScene(LevelScenesList.Instance.GetFirstSceneName());
        }
        
        public void HandleGoToNextLevel()
        {
            HandleUnPause();
            SceneLoader.LoadScene(LevelScenesList.Instance.GetNextSceneName());
        }
    }
}