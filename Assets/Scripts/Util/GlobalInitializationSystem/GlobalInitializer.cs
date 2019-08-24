using System.Collections.Generic;
using UnityEngine;

namespace Util.GlobalInitializationSystem
{
    public class GlobalInitializer : MonoBehaviour
    {
        [SerializeField]
        private SceneType m_SceneType;

        private List<IGlobalInitializable> m_Initializables;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Initializables = GlobalInitializerHelper.GetInitializablesForSceneType(m_SceneType);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (m_Initializables == null || m_Initializables.Count == 0)
            {
                return;
            }
            foreach (IGlobalInitializable globalInitializable in m_Initializables)
            {
                globalInitializable.Dispose();
            }
        }        
    }

    public enum SceneType
    {
        Game = 0,
        MainMenu = 1
    }
}