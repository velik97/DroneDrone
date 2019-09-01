using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace UnityEngine.UI
{
    [CreateAssetMenu(menuName = "Assets/SceneNames")]
    public class SceneNames : ScriptableObject
    {
        public static SceneNames Instance => AssetRoot.Instance.m_SceneNames;

        [SerializeField]
        private string m_MainMenuSceneName;
        [SerializeField]
        private List<string> m_LevelSceneNames;

        public bool HasNextLevelScene()
        {
            int index = m_LevelSceneNames.FindIndex(sceneName => SceneManager.GetActiveScene().name == sceneName) + 1;
            return m_LevelSceneNames.Count > index;
        }
        
        public string GetNextLevelSceneName()
        {
            int index = m_LevelSceneNames.FindIndex(sceneName => SceneManager.GetActiveScene().name == sceneName) + 1;
            return m_LevelSceneNames.Count > index ? m_LevelSceneNames[index] : null;
        }

        public string GetMainMenuSceneName()
        {
            return m_MainMenuSceneName;
        }

        public string GetLevelSceneName(int sceneIndex)
        {
            return sceneIndex >= GetLevelsCount() ? "" : m_LevelSceneNames[sceneIndex];
        }

        public int GetCurrentSceneIndex()
        {
            string currentName = SceneManager.GetActiveScene().name;

            if (currentName == m_MainMenuSceneName)
            {
                return -1;
            }

            return m_LevelSceneNames.IndexOf(currentName);
        }

        public int GetLevelsCount()
        {
            return m_LevelSceneNames.Count;
        }
    }
}