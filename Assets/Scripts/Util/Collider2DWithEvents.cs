using System;
using UnityEngine;

namespace Util
{
    public class Collider2DWithEvents : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEnter2DEvent;
        public event Action<Collider2D> OnTriggerStay2DEvent;
        public event Action<Collider2D> OnTriggerExit2DEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter2DEvent?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStay2DEvent?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExit2DEvent?.Invoke(other);
        }
    }
}