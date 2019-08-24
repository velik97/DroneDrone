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
            AngularForce = 0f;

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
                    AngularForce = PhysicsSettings.AngularForce;
                }
                else if (leftEngineIsOn)
                {
                    AngularForce = -PhysicsSettings.AngularForce;
                }

                DirectionalForce = PhysicsSettings.OneEngineLinearForce;
            }
            else if (rightEngineIsOn && leftEngineIsOn)
            {
                DirectionalForce = PhysicsSettings.TwoEnginesLinearForce;
            }
        }
    }
}