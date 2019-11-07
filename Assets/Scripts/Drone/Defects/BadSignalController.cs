using System;
using Drone.Defects;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Control
{
    public class BadSignalController : DisposableContainer, IEnginesStateModifier
    {
        private readonly LayerMask m_BadZoneLayerMask;
        
        private int m_BadSignalZonesTouching = 0;

        public BadSignalController(Collider2DWithEvents receiverCollider, LayerMask badZoneLayerMask)
        {
            m_BadZoneLayerMask = badZoneLayerMask;
            AddDisposable(receiverCollider.OnTriggerEnter2DCommand.Subscribe(OnReceiverEnteredCollider));
            AddDisposable(receiverCollider.OnTriggerExit2DCommand.Subscribe(OnReceiverEscapedCollider));
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
    }
}