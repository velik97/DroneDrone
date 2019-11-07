using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TouchControl
{
    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Image m_ArrowImage;

        [SerializeField]
        private Color m_HighlightedColor;
        [SerializeField]
        private Color m_UnhighlightedColor;

        private readonly BoolReactiveProperty m_IsPressed = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsPressed => m_IsPressed;

        private void Awake()
        {
            UpdateImageColor();
        }

        public void SetImage(Sprite sprite)
        {
            m_ArrowImage.sprite = sprite;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            m_IsPressed.Value = true;
            UpdateImageColor();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_IsPressed.Value = false;
            UpdateImageColor();
        }

        private void UpdateImageColor()
        {
            m_ArrowImage.color = m_IsPressed.Value ? m_HighlightedColor : m_UnhighlightedColor;
        }
    }
}