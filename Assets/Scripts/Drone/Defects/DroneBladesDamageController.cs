using System;
using Drone.Control;
using Drone.Physics;
using GameProcessManaging;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Defects
{
    public class DroneBladesDamageController : DisposableContainer, IEnginesStateModifier, IRestoreStateHandler
    {
        private readonly LayerMask m_ObstaclesLayerMask;
        
        private readonly GameObject m_RightBladesObject;
        private readonly GameObject m_LeftBladesObject;
        
        private readonly DronePhysicsBase m_DronePhysics;
        
        private bool m_RightEngineIsWorking = true;
        private bool m_LeftEngineIsWorking = true;

        public DroneBladesDamageController(Collider2DWithEvents rightBladesCollider,
            Collider2DWithEvents leftBladesCollider, LayerMask obstaclesLayerMask, GameObject rightBladesObject,
            GameObject leftBladesObject, DronePhysicsBase dronePhysics)
        {
            m_ObstaclesLayerMask = obstaclesLayerMask;
            m_RightBladesObject = rightBladesObject;
            m_LeftBladesObject = leftBladesObject;
            m_DronePhysics = dronePhysics;

            AddDisposable(rightBladesCollider.OnTriggerEnter2DCommand.Subscribe(CheckRightBladeForDamage));
            AddDisposable(leftBladesCollider.OnTriggerEnter2DCommand.Subscribe(CheckLeftBladeForDamage));

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
                    m_DronePhysics.ApplyBladeCollisionPokeForce(true);
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
                    m_DronePhysics.ApplyBladeCollisionPokeForce(false);
                    m_LeftBladesObject.SetActive(false);
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

        public void HandleRestoreState()
        {
            m_RightBladesObject.SetActive(true);
            m_LeftBladesObject.SetActive(true);

            m_RightEngineIsWorking = true;
            m_LeftEngineIsWorking = true;
        }
    }
}