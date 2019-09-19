using System;
using Drone.Control;
using GameProcessManaging;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Defects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DroneBladesDamageController : DisposableContainerMonoBehaviour, IEnginesStateModifier, IRestoreStateHandler
    {
        [SerializeField]
        private Collider2DWithEvents m_RightBladesCollider;
        [SerializeField]
        private Collider2DWithEvents m_LeftBladesCollider;
        [SerializeField]
        private LayerMask m_ObstaclesLayerMask;
        [SerializeField]
        private GameObject m_RightBladesObject;
        [SerializeField]
        private GameObject m_LeftBladesObject;

        [SerializeField]
        private float m_ImpactForce;

        private Rigidbody2D m_Rigidbody;
        
        private bool m_RightEngineIsWorking = true;
        private bool m_LeftEngineIsWorking = true;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            AddDisposable(m_RightBladesCollider.OnTriggerEnter2DEvent.Subscribe(CheckRightBladeForDamage));
            AddDisposable(m_LeftBladesCollider.OnTriggerEnter2DEvent.Subscribe(CheckLeftBladeForDamage));
            
            AddDisposable(EventBus.Subscribe(this));
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
                    ApplyImpactForce(true);
                    m_RightBladesObject.SetActive(false);
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
                    ApplyImpactForce(false);
                    m_LeftBladesObject.SetActive(false);
                }
                
                m_LeftEngineIsWorking = false;
            }
        }

        private void ApplyImpactForce(bool right)
        {
            Vector3 applyPosition = (right ? m_RightBladesObject : m_LeftBladesObject).transform.position;
            Vector3 force = (right ? -transform.right : transform.right) * m_ImpactForce;
            
            m_Rigidbody.AddForceAtPosition(force, applyPosition, ForceMode2D.Impulse);
        }

        public bool ModifiedRightEngineState()
        {
            return m_RightEngineIsWorking;
        }

        public bool ModifiedLeftEngineState()
        {
            return m_LeftEngineIsWorking;
        }

        public void HandleRestoreState()
        {
            m_RightBladesObject.SetActive(true);
            m_LeftBladesObject.SetActive(true);

            m_RightEngineIsWorking = true;
            m_LeftEngineIsWorking = true;
        }
    }
}