using System;

namespace Util.EventBusSystem
{
    public class DisposableEventBusSubscriber : IDisposable
    {
        private readonly IGlobalSubscriber m_GlobalSubscriber;

        public DisposableEventBusSubscriber(IGlobalSubscriber globalSubscriber)
        {
            m_GlobalSubscriber = globalSubscriber;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(m_GlobalSubscriber);
        }
    }
}