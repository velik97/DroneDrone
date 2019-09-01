using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.EventBusSystem;

namespace Util.GlobalInitializationSystem
{
    public class GlobalInitializer : DisposableMonoBehaviour, IDestroySceneHandler
    {
        [SerializeField]
        private SceneType m_SceneType;

        private List<IGlobalInitializable> m_Initializables;

        private void Awake()
        {
            AddDisposable(EventBus.Subscribe(this));
            CollectInitializables();
            InitializeInAwake();
        }

        private void Start()
        {
            InitializeInStart();
        }

        private void CollectInitializables()
        {
            m_Initializables = GlobalInitializerHelper.GetInitializablesForSceneType(m_SceneType);
        }

        private void InitializeInAwake()
        {
            for (int i = 0; i < 3; i++)
            {
                InitializePrior prior = (InitializePrior) i;
                foreach (IGlobalInitializable initializable in m_Initializables)
                {
                    if (initializable.InitializePrior == prior)
                    {
                        initializable.Initialize();
                        AddDisposable(initializable);
                    }
                }
            }
        }

        private void InitializeInStart()
        {
            for (int i = 3; i < 6; i++)
            {
                InitializePrior prior = (InitializePrior) i;
                foreach (IGlobalInitializable initializable in m_Initializables)
                {
                    if (initializable.InitializePrior == prior)
                    {
                        initializable.Initialize();
                        AddDisposable(initializable);
                    }
                }
            }
        }

        public void HandleDestroyScene()
        {
            Dispose();
        }
    }

    public enum SceneType
    {
        Game = 0,
        MainMenu = 1
    }

    public enum InitializePrior
    {
        EarlyAwake = 0,
        UsualAwake = 1,
        LateAwake = 2,
        EarlyStart = 3,
        UsualStart = 4,
        LateStart = 5
    }
}