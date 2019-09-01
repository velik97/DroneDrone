using System;
using System.Collections.Generic;
using Drone.Control;
using Drone.Defects;
using UnityEngine;

namespace Drone.Physics
{
    public class SimpleDronePhysicsWithEngineModifiers : DronePhysicsBase
    {
        private IEnginesStateModifier[] m_EnginesStateModifiers;

        protected override void Initialize()
        {
            base.Initialize();
            m_EnginesStateModifiers = GetComponents<IEnginesStateModifier>();
        }

        protected override void UpdateForces()
        {   
            DirectionalForce = 0f;
            Torque = 0f;

            bool rightEngineIsOn = RightEngineIsOn;
            bool leftEngineIsOn = LeftEngineIsOn;

            if (m_EnginesStateModifiers != null && m_EnginesStateModifiers.Length > 0)
            {
                foreach (var modifier in m_EnginesStateModifiers)
                {
                    rightEngineIsOn &= modifier.ModifiedRightEngineState();
                    leftEngineIsOn &= modifier.ModifiedLeftEngineState();
                }
            }
            
            //If only one of two engines is on 
            if (rightEngineIsOn ^ leftEngineIsOn)
            {
                if (rightEngineIsOn)
                {
                    Torque = PhysicsSettings.Torque;
                }
                else if (leftEngineIsOn)
                {
                    Torque = -PhysicsSettings.Torque;
                }

                DirectionalForce = PhysicsSettings.OneEngineLinearForce;
            }
            else if (rightEngineIsOn && leftEngineIsOn)
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