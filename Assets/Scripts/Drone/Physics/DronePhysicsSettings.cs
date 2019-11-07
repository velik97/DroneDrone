using UnityEngine;

namespace Drone.Physics
{
    [CreateAssetMenu(menuName = "Assets/Drone Physics Settings")]
    public class DronePhysicsSettings : ScriptableObject
    {
        public float Mass;
        
        public float OneEngineLinearForce;
        public float TwoEnginesLinearForce;
        public float Torque;

        public DragType DirectionalDragType;
        public float DirectionalDrag;
        
        public DragType AngularDragType;
        public float AngularDrag;

        public float MaxAngularSpeed;
        public AnimationCurve TorqueFromAngularSpeedCurve;

        public float BladeCollisionPokeForce;
    }

    public enum DragType
    {
        DefaultUnity = 0,
        Linear = 1,
        Quadratic = 2
    }
}