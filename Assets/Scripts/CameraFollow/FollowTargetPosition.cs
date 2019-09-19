using GameProcessManaging;
using UnityEngine;
using Util;
using Util.EventBusSystem;

namespace CameraFollow
{
    public class FollowTargetPosition : DisposableContainerMonoBehaviour, IRestoreStateHandler
    {
        [SerializeField]
        private Transform m_TargetTransform;
        
        [SerializeField]
        private float m_Speed;

        private Vector3 m_StartPosition;
        private Quaternion m_StartRotation;
        
        private Vector3 m_StartDirection;
        private Vector3 m_DirectionToTarget;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_StartDirection = m_TargetTransform.position - transform.position;
            m_StartPosition = transform.position;
            m_StartRotation = transform.rotation;
            
            AddDisposable(EventBus.Subscribe(this));
        }

        private void FixedUpdate()
        {
            UpdateDirection();
            UpdatePosition();
        }

        private void UpdateDirection()
        {
            m_DirectionToTarget = m_TargetTransform.position - transform.position;
            m_DirectionToTarget -= m_StartDirection;
        }

        private void UpdatePosition()
        {
            transform.position += m_DirectionToTarget * Time.fixedDeltaTime * m_Speed;
        }

        public void HandleRestoreState()
        {
            transform.position = m_StartPosition;
            transform.rotation = m_StartRotation;
        }
    }
}
