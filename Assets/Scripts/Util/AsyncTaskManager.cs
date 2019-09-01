using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// Gives opportunity to call methods parallel with calling callbacks after completion
    /// </summary>
    public class AsyncTaskManager
    {    
        /// <summary>
        /// Calls <paramref name="asyncTask"/> in a parallel thread, than calls <paramref name="callback"></paramref>
        /// in the main thread with result of <paramref name="asyncTask"/> as an argument
        /// </summary>
        /// <param name="asyncTask">to be called in a parallel thread</param>
        /// <param name="callback">to be called after <paramref name="asyncTask"/> is complete</param>
        /// <typeparam name="T">type, that <paramref name="asyncTask"/> returns</typeparam>
        public static void CallFuncParallel<T>(Func<T> asyncTask, Action<T> callback = null)
        {
            CoroutineHandler.CoroutineHandler.StartCoroutineOnHandler(AsyncTaskCoroutine<T>(asyncTask, callback));
        }
    
        /// <summary>
        /// Calls <paramref name="asyncTask"/> in a parallel thread, than calls <paramref name="callback"></paramref>
        /// in the main thread
        /// </summary>
        /// <param name="asyncTask">to be called in a parallel thread</param>
        /// <param name="callback">to be called after <paramref name="asyncTask"/> is complete</param>
        public static void CallFuncParallel(Action asyncTask, Action callback = null)
        {
            CoroutineHandler.CoroutineHandler.StartCoroutineOnHandler(AsyncTaskCoroutine(asyncTask, callback));
        }

        private static IEnumerator AsyncTaskCoroutine<T>(Func<T> asyncTask, Action<T> callback = null)
        {
            using (var task = new Task<T>(asyncTask))
            {
                task.Start();

                while (!(task.IsCanceled || task.IsCanceled || task.IsFaulted))
                {
                    yield return null;
                }

                if (task.IsFaulted || task.IsCanceled)
                {
                    if (task.Exception?.InnerExceptions != null)
                        foreach (var exception in task.Exception?.InnerExceptions)
                        {
                            Debug.LogError(exception);
                        }
                    yield break;
                }

                if (task.Result != null)
                    callback?.Invoke(task.Result);
            }
        }
    
        private static IEnumerator AsyncTaskCoroutine(Action asyncTask, Action callback = null)
        {
            using (Task task = new Task(asyncTask))
            {
                task.Start();

                while (!(task.IsCanceled || task.IsCanceled || task.IsFaulted))
                {
                    yield return null;
                }

                if (task.IsFaulted || task.IsCanceled)
                {
                    if (task.Exception?.InnerExceptions != null)
                        foreach (var exception in task.Exception?.InnerExceptions)
                        {
                            Debug.LogError(exception);
                        }
                    yield break;
                }

                callback?.Invoke();
            }
        }
    }
}
