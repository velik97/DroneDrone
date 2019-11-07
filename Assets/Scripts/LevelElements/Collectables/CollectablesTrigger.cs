using UniRx;
using UnityEngine;
using Util;

namespace LevelElements.Collectables
{
    public class CollectablesTrigger : DisposableContainer
    {
        public CollectablesTrigger(Collider2DWithEvents myCollider)
        {
            AddDisposable(myCollider.OnTriggerEnter2DCommand.Subscribe(OnCollierEntered));
        }

        private void OnCollierEntered(Collider2D other)
        {
            ICollectable collectable = other.GetComponent<ICollectable>();
            collectable?.Collect();
        }
    }
}