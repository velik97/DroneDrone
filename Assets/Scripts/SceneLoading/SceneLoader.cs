using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
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
            CoroutineHandler.Instance.StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private static IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                EventBus.TriggerEvent<IAsyncSceneLoadingProcessHandler>(h =>
                    h.HandleAsyncSceneLoadingProcess(operation.progress / 0.9f));
                yield return null;
            }
            EventBus.TriggerEvent<IAsyncSceneLoadingProcessHandler>(h =>
                h.HandleAsyncSceneLoadingProcess(1f));
            
            yield return new WaitUntil(() => s_WaitPredicates.Any(predicate => predicate()));
            EventBus.TriggerEvent<IDestroySceneHandler>(h => h.HandleDestroyScene());
            yield return null;
            
            operation.allowSceneActivation = true;

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        public static IDisposable AddWaitPredicate(Func<bool> predicate)
        {
            s_WaitPredicates.Add(predicate);
            return new DisposeAction(() => RemoveWaitPredicate(predicate));
        }

        private static void RemoveWaitPredicate(Func<bool> predicate)
        {
            s_WaitPredicates.Remove(predicate);
        }
    }
}