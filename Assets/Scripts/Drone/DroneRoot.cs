using Drone.Control;
using Drone.Defects;
using Drone.Physics;
using UnityEngine;
using Util;

namespace Drone
{
    public class DroneRoot : MonoBehaviour
    {
        [SerializeField]
        private DroneControl m_DroneControl;
        [SerializeField]
        private DronePhysicsBase m_DronePhysics;

        public DroneControl DroneControl => m_DroneControl;
        public DronePhysicsBase DronePhysics => m_DronePhysics;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_DroneControl.SubscribeOnControl(m_DronePhysics);
        }
    }
}