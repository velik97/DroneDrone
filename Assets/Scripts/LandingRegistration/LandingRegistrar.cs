using UnityEngine;
using Util.EventBusSystem;

namespace LandingRegistration
{
    [RequireComponent(typeof(Collider2D))]
    public class LandingRegistrar : MonoBehaviour
    {   
        [SerializeField]
        private string m_DroneFinishColliderTag;

        private ushort m_DroneRailsInTouchCount = 0;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(m_DroneFinishColliderTag))
            {
                m_DroneRailsInTouchCount++;

                if (m_DroneRailsInTouchCount == 2)
                {
                    EventBus.TriggerEvent<IFinishLandingHandler>(h => h.HandleStartedFinishLanding());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(m_DroneFinishColliderTag))
            {
                m_DroneRailsInTouchCount--;
                
                if (m_DroneRailsInTouchCount < 2)
                {
                    EventBus.TriggerEvent<IFinishLandingHandler>(h => h.HandleInterruptedFinishLanding());
                }
            }
        }
    }
}