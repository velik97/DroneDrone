using System;
using System.Collections;
using Drone.Defects;
using LevelProgression;
using UnityEngine;
using Util;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace GameProcessManaging
{
    public class GameOverAndFinishZoneController : IGlobalInitializableInGame, IDroneBladesDamageHandler, IDroneBadSignalZoneHandler, IFinishLandingHandler
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

        private readonly float m_GameOverCountDownTotalSeconds;
        private readonly float m_GameFinishCountDownTotalSeconds;

        private readonly IDisposable m_Subscription;

        public GameOverAndFinishZoneController()
        {
            m_GameOverCountDownTotalSeconds = GameOverAndFinishSettings.Instance.GameOverCountDown;
            m_GameFinishCountDownTotalSeconds = GameOverAndFinishSettings.Instance.GameFinishCountDown;
            
            m_Subscription = EventBus.Subscribe(this);

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
            
            m_GameOverCountDownCoroutine = GameOverCountDownCoroutine();
            CoroutineHandler.StartCoroutineOnHandler(m_GameOverCountDownCoroutine);
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
                
            CoroutineHandler.StopCoroutineOnHandler(m_GameOverCountDownCoroutine);
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
            
            m_GameFinishCountDownCoroutine = GameFinishCountDownCoroutine();
            CoroutineHandler.StartCoroutineOnHandler(m_GameFinishCountDownCoroutine);
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
            
            CoroutineHandler.StopCoroutineOnHandler(m_GameFinishCountDownCoroutine);
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

        private IEnumerator m_GameOverCountDownCoroutine;
        private IEnumerator m_GameFinishCountDownCoroutine;

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

        public void Dispose()
        {
            m_Subscription.Dispose();
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