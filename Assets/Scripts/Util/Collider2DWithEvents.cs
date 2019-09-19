using System;
using UnityEngine;

namespace Util
{
    public class Collider2DWithEvents : MonoBehaviour
    {
        public DisposableEvent<Collider2D> OnTriggerEnter2DEvent = new DisposableEvent<Collider2D>();
        public DisposableEvent<Collider2D> OnTriggerStay2DEvent = new DisposableEvent<Collider2D>();
        public DisposableEvent<Collider2D> OnTriggerExit2DEvent = new DisposableEvent<Collider2D>();

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