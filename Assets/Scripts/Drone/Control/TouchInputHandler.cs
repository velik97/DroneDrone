using System.Linq;
using UnityEngine;

namespace Drone.Control
{
    public class TouchInputHandler : IInputHandler
    {
        private readonly int m_DisplayXCoordinate;

        public TouchInputHandler()
        {
            m_DisplayXCoordinate = Screen.width / 2;
        }

        public bool RightIsPressed()
        {
            return Input.touches.Any(touch => touch.position.x > m_DisplayXCoordinate);
        }

        public bool LeftIsPressed()
        {
            return Input.touches.Any(touch => touch.position.x < m_DisplayXCoordinate);
        }
    }
}