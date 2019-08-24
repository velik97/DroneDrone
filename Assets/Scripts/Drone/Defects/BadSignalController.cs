using System;
using Drone.Defects;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Control
{
    public class BadSignalController : MonoBehaviour, IEnginesStateModifier
    {
        [SerializeField]
        private Collider2DWithEvents m_ReceiverCollider;
        [SerializeField]
        private LayerMask m_BadZoneLayerMask;
        
        private int m_BadSignalZonesTouching = 0;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {            
            m_ReceiverCollider.OnTriggerEnter2DEvent += OnReceiverEnteredCollider;
            m_ReceiverCollider.OnTriggerExit2DEvent += OnReceiverEscapedCollider;
        }

        private void OnReceiverEnteredCollider(Collider2D collider)
        {
            if (m_BadZoneLayerMask.IsInMask(collider.gameObject))
            {
                m_BadSignalZonesTouching++;
                if (m_BadSignalZonesTouching == 1)
                {
                    EventBus.TriggerEvent<IDroneBadSignalZoneHandler>(h => h.HandleEnteredBadSignalZone());
                }
            }
        }
        
        private void OnReceiverEscapedCollider(Collider2D collider)
        {
            if (m_BadZoneLayerMask.IsInMask(collider.gameObject))
            {
                m_BadSignalZonesTouching--;
                if (m_BadSignalZonesTouching == 0)
                {
                    EventBus.TriggerEvent<IDroneBadSignalZoneHandler>(h => h.HandleEscapedBadSignalZone());
                }
            }
        }
        
        public bool ModifiedRightEngineState()
        {
            return m_BadSignalZonesTouching == 0;
        }

        public bool ModifiedLeftEngineState()
        {
            return m_BadSignalZonesTouching == 0;
        }

        private void OnDestroy()
        {
            m_ReceiverCollider.OnTriggerEnter2DEvent -= OnReceiverEnteredCollider;
            m_ReceiverCollider.OnTriggerExit2DEvent -= OnReceiverEscapedCollider;
        }
    }
}