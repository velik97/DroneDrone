using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace GameProcessManaging
{
    public class GameStateManager : IGlobalInitializableInGame, IRestartGameHandler, IGamePauseHandler
    {
        private readonly IDisposable m_Subscription;
        
        public GameStateManager()
        {
            m_Subscription = EventBus.Subscribe(this);
        }
        
        public void HandleRestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void HandlePause()
        {
            Time.timeScale = 0f;
        }

        public void HandleUnPause()
        {
            Time.timeScale = 1f;
        }
        
        public void Dispose()
        {
            m_Subscription.Dispose();
        }
    }
}