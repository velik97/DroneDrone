using UnityEngine;

namespace Util.DebugUtils
{
    public class PerformanceWindow : MonoBehaviour
    {
        private static PerformanceWindow s_Instance;
        
        private float m_PassedTime = 0.0f;
        private int m_TicksBetweenFpsUpdates = 20;
        private int m_PassedTicks = 0;

        private float m_AvgFps = 0;
        private float m_AvgMSec = 0;

        private GUIStyle m_AvgStyle;
        private Rect m_AvgRect;

        private void Awake()
        {
#if !DEBUG
            Destroy(this);
            return;
#endif
            
            if (s_Instance == null)
            {
                s_Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this);
            CreateStyle();
        }

        private void CreateStyle()
        {
            int w = Screen.width, h = Screen.height;
 
            m_AvgStyle = new GUIStyle();
 
            m_AvgRect = new Rect(0, 0, w, h * 4 / 100);
            m_AvgStyle.alignment = TextAnchor.UpperLeft;
            m_AvgStyle.fontSize = h * 4 / 100;
            m_AvgStyle.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        }

        private void Update()
        {
            m_PassedTime += Time.unscaledDeltaTime;
            m_PassedTicks++;

            if (m_PassedTicks >= m_TicksBetweenFpsUpdates)
            {
                float secsBetweenUpdates = m_PassedTime / m_PassedTicks;
                m_AvgFps = 1 / secsBetweenUpdates;
                m_AvgMSec = secsBetweenUpdates * 1000f;

                m_PassedTicks = 0;
                m_PassedTime = 0f;
            }
        }

        private void OnGUI()
        {
            string text = $"{m_AvgMSec:0.0} ms ({m_AvgFps:0.} fps)";
            GUI.Label(m_AvgRect, text, m_AvgStyle);
        }
    }
}