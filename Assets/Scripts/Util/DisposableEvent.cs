using System;

namespace Util
{
    public class DisposableEvent<T>
    {
        private event Action<T> Event;

        public void Invoke(T obj)
        {
            Event?.Invoke(obj);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            Event += action;
            return new DisposeAction(() => Unsubscribe(action));
        }

        private void Unsubscribe(Action<T> action)
        {
            Event -= action;
        }
    }
    
    public class DisposableEvent
    {
        private event Action Event;

        public void Invoke()
        {
            Event?.Invoke();
        }

        public IDisposable Subscribe(Action action)
        {
            Event += action;
            return new DisposeAction(() => Unsubscribe(action));
        }

        private void Unsubscribe(Action action)
        {
            Event -= action;
        }
    }
}