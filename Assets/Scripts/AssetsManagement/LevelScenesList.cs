using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/Level Scenes List")]
    public class LevelScenesList : ScriptableObject
    {
        public static LevelScenesList Instance => AssetRoot.Instance.LevelScenesList;
        
        [SerializeField]
        private List<string> m_LevelSceneNames;

        public bool HasNextScene()
        {
            int index = m_LevelSceneNames.FindIndex(sceneName => SceneManager.GetActiveScene().name == sceneName) + 1;
            return m_LevelSceneNames.Count > index;
        }
        
        public string GetNextSceneName()
        {
            int index = m_LevelSceneNames.FindIndex(sceneName => SceneManager.GetActiveScene().name == sceneName) + 1;
            if (m_LevelSceneNames.Count > index)
            {
                return m_LevelSceneNames[index];
            }
            return null;
        }

        public string GetFirstSceneName()
        {
            return m_LevelSceneNames[0];
        }
    }
}