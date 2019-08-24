using System.Collections;

namespace Util
{
    public class CoroutineHandler : MonoSingleton<CoroutineHandler>
    {
        public static void StartCoroutineOnHandler(IEnumerator enumerator)
        {
            Instance.StartCoroutine(enumerator);
        }

        public static void StopCoroutineOnHandler(IEnumerator enumerator)
        {
            Instance.StopCoroutine(enumerator);
        }

        public static void StopAllCoroutinesOnHandler()
        {
            Instance.StopAllCoroutines();
        }
    }
}