using System;
using Drone.Control;
using Drone.Defects;
using Drone.Physics;
using UnityEngine;
using Util;

namespace Drone
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Drone : DisposableContainerMonoBehaviour
    {
        [SerializeField]
        private DroneReferences m_DroneReferences;
        [SerializeField]
        private DronePhysicsSettings m_DronePhysicsSettings;
        [SerializeField]
        private LayerMask m_BadSignalZoneLayerMask;
        [SerializeField]
        private LayerMask m_ObstacleLayerMask;

        private DroneControl m_DroneControl;
        private BadSignalController m_BadSignalController;
        private DroneBladesDamageController m_DroneBladesDamageController;
        private SimpleDronePhysicsWithEngineModifiers m_DronePhysics;

        private void Awake()
        {
            Initialize();   
        }

        private void Initialize()
        {
            m_DroneControl = new DroneControl();
            m_BadSignalController = new BadSignalController(
                m_DroneReferences.ReceiverCollider,
                m_BadSignalZoneLayerMask);
            m_DronePhysics = new SimpleDronePhysicsWithEngineModifiers(
                m_DronePhysicsSettings,
                transform,
                GetComponent<Rigidbody2D>());
            m_DroneBladesDamageController = new DroneBladesDamageController(
                m_DroneReferences.RightBladesCollider,
                m_DroneReferences.LeftBladesCollider,
                m_ObstacleLayerMask,
                m_DroneReferences.RightBladesObject,
                m_DroneReferences.LeftBladesObject,
                m_DronePhysics);
            
            m_DronePhysics.SetEngineModifiers(m_BadSignalController, m_DroneBladesDamageController);
            m_DronePhysics.SubscribeOnControl(m_DroneControl);
        }

        private void Update()
        {
            m_DroneControl.Update();
        }

        private void FixedUpdate()
        {
            m_DronePhysics.FixedUpdate();
        }

        [Serializable]
        private class DroneReferences
        {
            public Collider2DWithEvents ReceiverCollider;
            public Collider2DWithEvents RightBladesCollider;
            public Collider2DWithEvents LeftBladesCollider;
            public GameObject RightBladesObject;
            public GameObject LeftBladesObject;
        }
    }
}