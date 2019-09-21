using System;
using System.Collections;

namespace Util.CoroutineHandler
{
    public class CoroutineHandler : MonoSingleton<CoroutineHandler>
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public static IDisposable StartCoroutineOnHandler(IEnumerator enumerator)
        {
            DisposableEnumerator disposableEnumerator = new DisposableEnumerator(enumerator, StopCoroutineOnHandler);
            Instance.StartCoroutine(disposableEnumerator);
            return disposableEnumerator;
        }

        private static void StopCoroutineOnHandler(IEnumerator enumerator)
        {
            Instance.StopCoroutine(enumerator);
        }
    }
}