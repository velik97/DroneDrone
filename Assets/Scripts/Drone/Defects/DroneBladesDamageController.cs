using System;
using Drone.Control;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Defects
{
    public class DroneBladesDamageController : MonoBehaviour, IEnginesStateModifier
    {
        [SerializeField]
        private Collider2DWithEvents m_RightBladesCollider;
        [SerializeField]
        private Collider2DWithEvents m_LeftBladesCollider;
        [SerializeField]
        private LayerMask m_ObstaclesLayerMask;
        
        private bool m_RightEngineIsWorking = true;
        private bool m_LeftEngineIsWorking = true;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {            
            m_RightBladesCollider.OnTriggerEnter2DEvent += CheckRightBladeForDamage;
            m_LeftBladesCollider.OnTriggerEnter2DEvent += CheckLeftBladeForDamage;
        }

        private void CheckRightBladeForDamage(Collider2D touchedCollider)
        {
            if (m_ObstaclesLayerMask.IsInMask(touchedCollider.gameObject))
            {                
                if (m_RightEngineIsWorking)
                {
                    if (m_LeftEngineIsWorking)
                    {
                        EventBus.TriggerEvent<IDroneBladesDamageHandler>(h => h.HandleOneEngineIsBroken());
                    }
                    else
                    {
                        EventBus.TriggerEvent<IDroneBladesDamageHandler>(h => h.HandleBothEnginesAreBroken());
                    }
                }
                
                m_RightEngineIsWorking = false;
            }
        }
        
        private void CheckLeftBladeForDamage(Collider2D touchedCollider)
        {
            if (m_ObstaclesLayerMask.IsInMask(touchedCollider.gameObject))
            {
                if (m_LeftEngineIsWorking)
                {
                    if (m_RightEngineIsWorking)
                    {
                        EventBus.TriggerEvent<IDroneBladesDamageHandler>(h => h.HandleOneEngineIsBroken());
                    }
                    else
                    {
                        EventBus.TriggerEvent<IDroneBladesDamageHandler>(h => h.HandleBothEnginesAreBroken());
                    }
                }
                
                m_LeftEngineIsWorking = false;
            }
        }

        public bool ModifiedRightEngineState()
        {
            return m_RightEngineIsWorking;
        }

        public bool ModifiedLeftEngineState()
        {
            return m_LeftEngineIsWorking;
        }

        private void OnDestroy()
        {
            m_RightBladesCollider.OnTriggerEnter2DEvent -= CheckRightBladeForDamage;
            m_LeftBladesCollider.OnTriggerEnter2DEvent -= CheckLeftBladeForDamage;        
        }
    }
}