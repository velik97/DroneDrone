using System.Linq;
using UnityEngine;

namespace Drone.Control
{
    public class KeyboardInputReader : IInputReader
    {
        private KeyCode[] m_RightEngineKeys;
        private KeyCode[] m_LeftEngineKeys;

        public KeyboardInputReader()
        {
            m_RightEngineKeys = KeyboardControlSettings.Instance.RightEngineKeys;
            m_LeftEngineKeys = KeyboardControlSettings.Instance.LeftEngineKeys;
        }

        public bool RightIsPressed()
        {
            return m_RightEngineKeys.Any(Input.GetKey);
        }

        public bool LeftIsPressed()
        {
            return m_LeftEngineKeys.Any(Input.GetKey);
        }
    }
}