using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.CoroutineHandler;
using Util.EventBusSystem;
using Util.GlobalInitializationSystem;

namespace SceneLoading
{
    public static class SceneLoader
    {
        private static List<Func<bool>> s_WaitPredicates = new List<Func<bool>>();
        
        public static void LoadScene(string sceneName)
        {
            EventBus.TriggerEvent<IDestroySceneHandler>(h => h.HandleDestroyScene());
            CoroutineHandler.Instance.StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        public static void LoadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        private static IEnumerator LoadSceneCoroutine(string sceneName)
        {
            yield return new WaitUntil(() => s_WaitPredicates.Any(predicate => predicate()));
            LoadSceneInternal(sceneName);
        }
            
        private static void LoadSceneInternal(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static IDisposable AddWaitPredicate(Func<bool> predicate)
        {
            s_WaitPredicates.Add(predicate);
            return new DisposableWait(predicate);
        }

        private static void RemoveWaitPredicate(Func<bool> predicate)
        {
            s_WaitPredicates.Remove(predicate);
        }
        
        public class DisposableWait : IDisposable
        {
            private readonly Func<bool> m_WaitPredicates;

            public DisposableWait(Func<bool> waitPredicates)
            {
                m_WaitPredicates = waitPredicates;
            }

            public void Dispose()
            {
                RemoveWaitPredicate(m_WaitPredicates);
            }
        }
    }
}