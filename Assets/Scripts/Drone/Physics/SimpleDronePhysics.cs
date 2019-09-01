using System;
using UnityEngine;

namespace Drone.Physics
{
    public class SimpleDronePhysics : DronePhysicsBase
    {
        protected override void UpdateForces()
        {
            DirectionalForce = 0f;
            Torque = 0f;
            
            //If only one of two engines is on 
            if (RightEngineIsOn ^ LeftEngineIsOn)
            {
                if (RightEngineIsOn)
                {
                    Torque = PhysicsSettings.Torque;
                }
                else if (LeftEngineIsOn)
                {
                    Torque = -PhysicsSettings.Torque;
                }

                DirectionalForce = PhysicsSettings.OneEngineLinearForce;
            }
            else if (RightEngineIsOn && LeftEngineIsOn)
            {
                DirectionalForce = PhysicsSettings.TwoEnginesLinearForce;
            }

            if (Torque != 0f)
            {
                Torque = ModifiedTorque();
            }
        }
        
        private float ModifiedTorque()
        {
            float sign = Math.Sign(Torque);
            float curveValue = Mathf.Clamp01((sign * AngularVelocity/ PhysicsSettings.MaxAngularSpeed + 1) / 2f);
            return PhysicsSettings.TorqueFromAngularSpeedCurve.Evaluate(curveValue) * PhysicsSettings.Torque * sign;
        }
    }
}