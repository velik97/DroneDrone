using System;
using System.Collections;
using Drone.Defects;
using LandingRegistration;
using UnityEngine;
using Util;
using Util.CoroutineHandler;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace GameProcessManaging
{
    public class GameOverAndFinishController : DisposableContainer, 
        IGlobalInitializableInGame,
        IDroneBladesDamageHandler,
        IDroneBadSignalZoneHandler,
        IFinishLandingHandler,
        IRestoreStateHandler
    {
        private bool m_EnginesAreBroken = false;
        private bool m_IsInBadSignalZone = false;
        private bool m_IsLanding = false;
        private bool m_IsGameOver = false;
        private bool m_IsGameFinished = false;

        private bool m_IsCountingGameOver = false;
        private bool m_IsCountingGameFinish = false;

        private GameState m_GameState;

        private float m_GameOverTimePassed = 0f;
        private float m_GameFinishTimePassed = 0f;

        private float m_GameOverCountDownTotalSeconds;
        private float m_GameFinishCountDownTotalSeconds;
        
        private IDisposable m_GameOverCountDownEnumerator;
        private IDisposable m_GameFinishCountDownEnumerator;

        public GameOverAndFinishController()
        {
        }

        public InitializePrior InitializePrior => InitializePrior.UsualAwake;

        public void Initialize()
        {
            m_GameOverCountDownTotalSeconds = GameOverAndFinishSettings.Instance.GameOverCountDown;
            m_GameFinishCountDownTotalSeconds = GameOverAndFinishSettings.Instance.GameFinishCountDown;
            
            AddDisposable(EventBus.Subscribe(this));

            m_GameState = GameState.Playing;
        }
        
        public void HandleOneEngineIsBroken()
        {
        }

        public void HandleBothEnginesAreBroken()
        {
            m_EnginesAreBroken = true;
            UpdateState();
        }

        public void HandleEnteredBadSignalZone()
        {
            m_IsInBadSignalZone = true;
            UpdateState();
        }

        public void HandleEscapedBadSignalZone()
        {
            m_IsInBadSignalZone = false;
            UpdateState();
        }

        public void HandleStartedFinishLanding()
        {
            m_IsLanding = true;
            UpdateState();
        }

        public void HandleInterruptedFinishLanding()
        {
            m_IsLanding = false;
            UpdateState();
        }

        private void UpdateState()
        {
            GameState newGameState;

            if (m_IsGameFinished)
                newGameState = GameState.Won;
            else if (m_IsGameOver)
                newGameState = GameState.Lost;
            else if (m_IsLanding)
                newGameState = GameState.Finishing;
            else if (m_EnginesAreBroken || m_IsInBadSignalZone)
                newGameState = GameState.Losing;
            else
                newGameState = GameState.Playing;

            if (newGameState == m_GameState)
                return;
            
            m_GameState = newGameState;
            switch (m_GameState)
            {
                case GameState.Playing:
                    StopGameFinishCountDown();
                    StopGameOverCountDown();
                    break;
                case GameState.Losing:
                    StopGameFinishCountDown();
                    StartGameOverCountDown();
                    break;
                case GameState.Finishing:
                    StopGameOverCountDown();
                    StartGameFinishCountDown();
                    break;
                case GameState.Lost:
                    Lose();
                    break;
                case GameState.Won:
                    Win();
                    break;
            }     
        }
        
        private void StartGameOverCountDown()
        {
            if (m_IsCountingGameOver)
            {
                return;
            }
            m_IsCountingGameOver = true;
            
            m_GameOverTimePassed = 0f;
            EventBus.TriggerEvent<IGameOverCountDownHandler>(h => h.HandleStartGameOverCountDown());
            EventBus.TriggerEvent<IGameOverCountDownPercentageHandler>(h => h.HandleGameOverCountDownPercentageChanged(0f));
            
            m_GameOverCountDownEnumerator = AddDisposable(CoroutineHandler.StartCoroutineOnHandler(GameOverCountDownCoroutine()));
        }

        private void StopGameOverCountDown()
        {
            if (!m_IsCountingGameOver)
            {
                return;
            }
            m_IsCountingGameOver = false;
            
            m_GameOverTimePassed = 0f;
            EventBus.TriggerEvent<IGameOverCountDownHandler>(h => h.HandleInterruptGameOverCountDown());
            EventBus.TriggerEvent<IGameOverCountDownPercentageHandler>(h => h.HandleGameOverCountDownPercentageChanged(0f));
                
            m_GameOverCountDownEnumerator.Dispose();
        }

        private void StartGameFinishCountDown()
        {
            if (m_IsCountingGameFinish)
            {
                return;
            }
            m_IsCountingGameFinish = true;

            m_GameFinishTimePassed = 0f;
            EventBus.TriggerEvent<IGameFinishCountDownHandler>(h => h.HandleStartGameFinishCountDown());
            EventBus.TriggerEvent<IGameFinishCountDownPercentageHandler>(h => h.HandleGameFinishCountDownPercentageChanged(0f));
            
            m_GameFinishCountDownEnumerator = AddDisposable(CoroutineHandler.StartCoroutineOnHandler(GameFinishCountDownCoroutine()));
        }

        private void StopGameFinishCountDown()
        {
            if (!m_IsCountingGameFinish)
            {
                return;
            }
            m_IsCountingGameFinish = false;
            
            m_GameFinishTimePassed = 0f;
            EventBus.TriggerEvent<IGameFinishCountDownHandler>(h => h.HandleInterruptGameFinishCountDown());
            EventBus.TriggerEvent<IGameFinishCountDownPercentageHandler>(h => h.HandleGameFinishCountDownPercentageChanged(0f));
            
            m_GameFinishCountDownEnumerator.Dispose();
        }

        private void Win()
        {
            EventBus.TriggerEvent<IGameFinishCountDownHandler>(h => h.HandleCompleteGameFinishCountDown());
            EventBus.TriggerEvent<IGameFinishHandler>(h => h.HandleGameFinish());
        }

        private void Lose()
        {
            EventBus.TriggerEvent<IGameOverCountDownHandler>(h => h.HandleCompleteGameOverCountDown());
            EventBus.TriggerEvent<IGameOverHandler>(h => h.HandleGameOver());

        }

        private IEnumerator GameOverCountDownCoroutine()
        {
            while (m_GameOverTimePassed < m_GameOverCountDownTotalSeconds)
            {
                m_GameOverTimePassed += Time.deltaTime;
                float percentage = m_GameOverTimePassed / m_GameOverCountDownTotalSeconds;
                EventBus.TriggerEvent<IGameOverCountDownPercentageHandler>(h
                    => h.HandleGameOverCountDownPercentageChanged(percentage));
                yield return null;
            }
            EventBus.TriggerEvent<IGameOverCountDownPercentageHandler>(h => h.HandleGameOverCountDownPercentageChanged(1f));

            m_IsGameOver = true;
            UpdateState();
        }

        private IEnumerator GameFinishCountDownCoroutine()
        {
            while (m_GameFinishTimePassed < m_GameFinishCountDownTotalSeconds)
            {
                m_GameFinishTimePassed += Time.deltaTime;
                float percentage = m_GameFinishTimePassed / m_GameFinishCountDownTotalSeconds;
                EventBus.TriggerEvent<IGameFinishCountDownPercentageHandler>(h
                    => h.HandleGameFinishCountDownPercentageChanged(percentage));
                yield return null;
            }
            EventBus.TriggerEvent<IGameFinishCountDownPercentageHandler>(h => h.HandleGameFinishCountDownPercentageChanged(1f));
            
            m_IsGameFinished = true;
            UpdateState();
        }

        public void HandleRestoreState()
        {
            m_EnginesAreBroken = false;
            m_IsInBadSignalZone = false;
            m_IsLanding = false;
            m_IsGameOver = false;
            m_IsGameFinished = false;
            
            UpdateState();
        }
    }

    public enum GameState
    {
        Playing = 0,
        Losing = 1,
        Finishing = 2,
        Lost = 3,
        Won = 4
    }
}