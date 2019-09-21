using System;
using UniRx;
using UnityEngine;

namespace Util
{
    public class Collider2DWithEvents : MonoBehaviour
    {
        public ReactiveCommand<Collider2D> OnTriggerEnter2DCommand = new ReactiveCommand<Collider2D>();
        public ReactiveCommand<Collider2D> OnTriggerStay2DCommand = new ReactiveCommand<Collider2D>();
        public ReactiveCommand<Collider2D> OnTriggerExit2DCommand = new ReactiveCommand<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter2DCommand.Execute(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStay2DCommand.Execute(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExit2DCommand.Execute(other);
        }
    }
}