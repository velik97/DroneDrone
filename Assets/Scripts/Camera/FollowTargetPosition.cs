using UnityEngine;

namespace Camera
{
    public class FollowTargetPosition : MonoBehaviour
    {
        [SerializeField]
        private Transform m_TargetTransform;
        
        [SerializeField]
        private float m_Speed;

        private Vector3 m_StartDirection;
        private Vector3 m_DirectionToTarget;

        private void Awake()
        {
            m_StartDirection = m_TargetTransform.position - transform.position;
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
    }
}
