using UnityEngine;

namespace Drone.Physics
{
    [CreateAssetMenu(menuName = "Assets/Drone Physics Settings")]
    public class DronePhysicsSettings : ScriptableObject
    {
        [SerializeField]
        private float m_Mass;
        
        [SerializeField]
        private float m_OneEngineLinearForce;
        [SerializeField]
        private float m_TwoEnginesLinearForce;
        [SerializeField]
        private float m_AngularForce;

        [SerializeField]
        private DragType m_DirectionalDragType;
        [SerializeField]
        private float m_DirectionalDrag;
        
        [SerializeField]
        private DragType m_AngularDragType;
        [SerializeField]
        private float m_AngularDrag;

        public float Mass => m_Mass;

        public float OneEngineLinearForce => m_OneEngineLinearForce;
        public float TwoEnginesLinearForce => m_TwoEnginesLinearForce;
        public float AngularForce => m_AngularForce;

        public DragType DirectionalDragType => m_DirectionalDragType;
        public float DirectionalDrag => m_DirectionalDrag;
        
        public DragType AngularDragType => m_AngularDragType;
        public float AngularDrag => m_AngularDrag;
    }

    public enum DragType
    {
        DefaultUnity = 0,
        Linear = 1,
        Quadratic = 2
    }
}