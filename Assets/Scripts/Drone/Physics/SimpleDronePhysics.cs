namespace Drone.Physics
{
    public class SimpleDronePhysics : DronePhysicsBase
    {
        protected override void UpdateForces()
        {
            DirectionalForce = 0f;
            AngularForce = 0f;
            
            //If only one of two engines is on 
            if (RightEngineIsOn ^ LeftEngineIsOn)
            {
                if (RightEngineIsOn)
                {
                    AngularForce = PhysicsSettings.AngularForce;
                }
                else if (LeftEngineIsOn)
                {
                    AngularForce = -PhysicsSettings.AngularForce;
                }

                DirectionalForce = PhysicsSettings.OneEngineLinearForce;
            }
            else if (RightEngineIsOn && LeftEngineIsOn)
            {
                DirectionalForce = PhysicsSettings.TwoEnginesLinearForce;
            }
        }
    }
}