using System;
using Drone.Control;
using UnityEngine;

namespace Drone.Physics
{
    public class SimpleDronePhysics : DronePhysicsBase
    {
        public SimpleDronePhysics(DronePhysicsSettings physicsSettings, Transform transform, Rigidbody2D rigidbody) : base(physicsSettings, transform, rigidbody)
        {}

        protected override void UpdateForces()
        {
            DirectionalForce = 0f;
            Torque = 0f;
            
            //If only one of two engines is on 
            if (RightEngineIsOn ^ LeftEngineIsOn)
            {
                if (RightEngineIsOn)
                {
                    Torque = m_PhysicsSettings.Torque;
                }
                else if (LeftEngineIsOn)
                {
                    Torque = -m_PhysicsSettings.Torque;
                }

                DirectionalForce = m_PhysicsSettings.OneEngineLinearForce;
            }
            else if (RightEngineIsOn && LeftEngineIsOn)
            {
                DirectionalForce = m_PhysicsSettings.TwoEnginesLinearForce;
            }

            if (Torque != 0f)
            {
                Torque = ModifiedTorque();
            }
        }
    }
}