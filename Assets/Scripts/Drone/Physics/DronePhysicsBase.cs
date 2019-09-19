using System;
using Drone.Control;
using GameProcessManaging;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Physics
{
    [RequireComponent(typeof(Rigidbody2D), typeof(DroneControl))]
    public abstract class DronePhysicsBase : DisposableContainerMonoBehaviour, IRestoreStateHandler
    {
        [SerializeField]
        protected DronePhysicsSettings PhysicsSettings;

        private Rigidbody2D m_Rigidbody;
        private DroneControl m_Control;

        protected bool RightEngineIsOn = false;
        protected bool LeftEngineIsOn = false;
        
        protected float Torque = 0f;
        protected float DirectionalForce = 0f;

        protected float AngularVelocity => m_Rigidbody?.angularVelocity ?? 0f;

        private Vector3 m_StartPosition;
        private Quaternion m_StartRotation;

        private void Awake()
        {
            Initialize();
        }
        
        private void FixedUpdate()
        {
            UpdateForces();
            ApplyForces();
        }

        public void TurnOnRight()
        {
            RightEngineIsOn = true;
        }
        
        public void TurnOnLeft()
        {
            LeftEngineIsOn = true;
        }
        
        public void TurnOffRight()
        {
            RightEngineIsOn = false;
        }
        
        public void TurnOffLeft()
        {
            LeftEngineIsOn = false;
        }

        protected virtual void Initialize()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            SetRigidBodyParams();

            DroneControl droneControl = GetComponent<DroneControl>();
            droneControl.SubscribeOnControl(this);

            m_StartPosition = transform.position;
            m_StartRotation = transform.rotation;
            
            AddDisposable(EventBus.Subscribe(this));
        }

        private void SetRigidBodyParams()
        {
            m_Rigidbody.mass = PhysicsSettings.Mass;
            m_Rigidbody.drag = PhysicsSettings.DirectionalDragType == DragType.DefaultUnity
                ? PhysicsSettings.DirectionalDrag
                : 0f;
            m_Rigidbody.angularDrag = PhysicsSettings.AngularDragType == DragType.DefaultUnity
                ? PhysicsSettings.AngularDrag
                : 0f;
        }

        protected abstract void UpdateForces();

        private void ApplyForces()
        {   
            Vector2 up = transform.up;
            m_Rigidbody.AddForce(up * DirectionalForce);
            m_Rigidbody.AddTorque(Torque);

            ApplyDrag();
        }

        private void ApplyDrag()
        {
            Vector2 velocity = m_Rigidbody.velocity;
            float angularVelocity = m_Rigidbody.angularVelocity;
            
            switch (PhysicsSettings.DirectionalDragType)
            {
                case DragType.DefaultUnity:
                    break;
                case DragType.Linear:
                    m_Rigidbody.AddForce(-velocity * PhysicsSettings.DirectionalDrag);
                    break;
                case DragType.Quadratic:
                    float magnitude = velocity.magnitude;
                    Vector2 normalized = velocity.normalized;
                    m_Rigidbody.AddForce(-normalized * magnitude *  magnitude * PhysicsSettings.DirectionalDrag);
                    break;
            }
            
            switch (PhysicsSettings.AngularDragType)
            {
                case DragType.DefaultUnity:
                    break;
                case DragType.Linear:
                    m_Rigidbody.AddTorque(-angularVelocity * PhysicsSettings.AngularDrag);
                    break;
                case DragType.Quadratic:
                    float sign = Mathf.Sign(angularVelocity);
                    m_Rigidbody.AddTorque(- sign * angularVelocity * angularVelocity * PhysicsSettings.AngularDrag);
                    break;
            }
        }

        public void HandleRestoreState()
        {
            transform.position = m_StartPosition;
            transform.rotation = m_StartRotation;
            
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularVelocity = 0f;
        }
    }
}
