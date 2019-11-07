using System;
using Drone.Control;
using GameProcessManaging;
using UniRx;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace Drone.Physics
{
    [RequireComponent(typeof(Rigidbody2D), typeof(DroneControl))]
    public abstract class DronePhysicsBase : DisposableContainer, IRestoreStateHandler
    {
        protected DronePhysicsSettings m_PhysicsSettings;

        private Transform m_Transform;
        private Rigidbody2D m_Rigidbody;

        protected bool RightEngineIsOn = false;
        protected bool LeftEngineIsOn = false;
        
        protected float Torque = 0f;
        protected float DirectionalForce = 0f;

        protected float AngularVelocity => m_Rigidbody?.angularVelocity ?? 0f;

        private Vector3 m_StartPosition;
        private Quaternion m_StartRotation;

        public DronePhysicsBase(DronePhysicsSettings physicsSettings, Transform transform, Rigidbody2D rigidbody)
        {
            m_PhysicsSettings = physicsSettings;
            m_Transform = transform;
            m_Rigidbody = rigidbody;

            SetRigidBodyParams();

            m_StartPosition = transform.position;
            m_StartRotation = transform.rotation;

            AddDisposable(EventBus.Subscribe(this));
        }

        public void SubscribeOnControl(DroneControl control)
        {
            AddDisposable(control.RightIsPressed.Subscribe(value => RightEngineIsOn = value));
            AddDisposable(control.LeftIsPressed.Subscribe(value => LeftEngineIsOn = value));
        }

        public void FixedUpdate()
        {
            UpdateForces();
            ApplyForces();
        }
        
        public void ApplyBladeCollisionPokeForce(bool right)
        {
            Vector3 force = (right ? Vector3.left : Vector3.right) * m_PhysicsSettings.BladeCollisionPokeForce;
            
            m_Rigidbody.AddRelativeForce(force, ForceMode2D.Impulse);
        }

        private void SetRigidBodyParams()
        {
            m_Rigidbody.mass = m_PhysicsSettings.Mass;
            m_Rigidbody.drag = m_PhysicsSettings.DirectionalDragType == DragType.DefaultUnity
                ? m_PhysicsSettings.DirectionalDrag
                : 0f;
            m_Rigidbody.angularDrag = m_PhysicsSettings.AngularDragType == DragType.DefaultUnity
                ? m_PhysicsSettings.AngularDrag
                : 0f;
        }

        protected abstract void UpdateForces();

        private void ApplyForces()
        {   
            Vector2 up = m_Transform.up;
            m_Rigidbody.AddForce(up * DirectionalForce);
            m_Rigidbody.AddTorque(Torque);

            ApplyDrag();
        }

        private void ApplyDrag()
        {
            Vector2 velocity = m_Rigidbody.velocity;
            float angularVelocity = m_Rigidbody.angularVelocity;
            
            switch (m_PhysicsSettings.DirectionalDragType)
            {
                case DragType.DefaultUnity:
                    break;
                case DragType.Linear:
                    m_Rigidbody.AddForce(-velocity * m_PhysicsSettings.DirectionalDrag);
                    break;
                case DragType.Quadratic:
                    float magnitude = velocity.magnitude;
                    Vector2 normalized = velocity.normalized;
                    m_Rigidbody.AddForce(-normalized * magnitude *  magnitude * m_PhysicsSettings.DirectionalDrag);
                    break;
            }
            
            switch (m_PhysicsSettings.AngularDragType)
            {
                case DragType.DefaultUnity:
                    break;
                case DragType.Linear:
                    m_Rigidbody.AddTorque(-angularVelocity * m_PhysicsSettings.AngularDrag);
                    break;
                case DragType.Quadratic:
                    float sign = Mathf.Sign(angularVelocity);
                    m_Rigidbody.AddTorque(- sign * angularVelocity * angularVelocity * m_PhysicsSettings.AngularDrag);
                    break;
            }
        }
        
        protected float ModifiedTorque()
        {
            float sign = Math.Sign(Torque);
            float curveValue = Mathf.Clamp01((sign * AngularVelocity/ m_PhysicsSettings.MaxAngularSpeed + 1) / 2f);
            return m_PhysicsSettings.TorqueFromAngularSpeedCurve.Evaluate(curveValue) * m_PhysicsSettings.Torque * sign;
        }

        public void HandleRestoreState()
        {
            m_Transform.position = m_StartPosition;
            m_Transform.rotation = m_StartRotation;
            
            m_Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.angularVelocity = 0f;
        }
    }
}
