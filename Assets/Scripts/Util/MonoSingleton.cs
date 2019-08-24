using UnityEngine;

namespace Util
{
    public abstract class MonoSingleton <T> : MonoBehaviour where T : MonoBehaviour {

        private static T instance;

        public static T Instance {
            get {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1 )
                    {
                        Debug.LogError($"[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton of type " + typeof(T).ToString () +
                                       "! Reopening the scene might fix it.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        Debug.LogWarning("[Singleton] There is no object of type " + typeof(T).ToString() + ". " + 
                                         "Creating new.");
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }

                return instance;
            }
        }


    }
}