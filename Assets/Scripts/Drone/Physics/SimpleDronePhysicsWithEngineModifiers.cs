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

        public SimpleDronePhysicsWithEngineModifiers(DronePhysicsSettings physicsSettings, Transform transform,
            Rigidbody2D rigidbody) :
            base(physicsSettings, transform, rigidbody)
        {}

        public void SetEngineModifiers(params IEnginesStateModifier[] enginesStateModifiers)
        {
            m_EnginesStateModifiers = enginesStateModifiers;
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
                    Torque = m_PhysicsSettings.Torque;
                }
                else if (leftEngineIsOn)
                {
                    Torque = -m_PhysicsSettings.Torque;
                }

                DirectionalForce = m_PhysicsSettings.OneEngineLinearForce;
            }
            else if (rightEngineIsOn && leftEngineIsOn)
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