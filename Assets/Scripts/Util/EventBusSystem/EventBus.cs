using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.EventBusSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<GlobalSubscriberNode>> s_GlobalSubscribers 
            = new Dictionary<Type, List<GlobalSubscriberNode>>();

        private static object m_Lock = new object();

        private static EventBusCleaner m_EventBusCleaner;

        static EventBus()
        {
            m_EventBusCleaner = new EventBusCleaner(s_GlobalSubscribers, m_Lock, 5f);
        }

        public static IDisposable Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> implementedGlobalSubscribers = EventBusHelper.GetImplementedGlobalSubscribers(subscriber);

            lock (m_Lock)
            {
                foreach (Type interfaceType in implementedGlobalSubscribers)
                {
                    List<GlobalSubscriberNode> correspondingList;
                    if (s_GlobalSubscribers.ContainsKey(interfaceType))
                    {
                        correspondingList = s_GlobalSubscribers[interfaceType];
                    }
                    else
                    {
                        correspondingList = new List<GlobalSubscriberNode>();
                        s_GlobalSubscribers[interfaceType] = correspondingList;
                    }

                    correspondingList.Add(new GlobalSubscriberNode(subscriber));
                }
            }

            return new DisposableEventBusSubscriber(subscriber);
        }
        
        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> implementedGlobalSubscribers = EventBusHelper.GetImplementedGlobalSubscribers(subscriber);

            lock (m_Lock)
            {
                foreach (Type interfaceType in implementedGlobalSubscribers)
                {
                    if (s_GlobalSubscribers.ContainsKey(interfaceType))
                    {
                        s_GlobalSubscribers[interfaceType]
                            .FirstOrDefault(node => node.SubscriberEquals(subscriber))
                            ?.Release();
                    }
                }
            }
        }

        public static void TriggerEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : IGlobalSubscriber
        {
            List<Type> nestedInterfaces = EventBusHelper.GetNestedInterfaces(typeof(TSubscriber));

            lock (m_Lock)
            {
                foreach (Type nestedInterface in nestedInterfaces)
                {
                    if (s_GlobalSubscribers.ContainsKey(nestedInterface))
                    {
                        foreach (GlobalSubscriberNode globalSubscriber in s_GlobalSubscribers[nestedInterface])
                        {
                            if (!globalSubscriber.IsReleased)
                            {
                                globalSubscriber.TriggerEventOnSubscriber(action);
                            }
                        }
                    }
                }
            }
        }
        
        public class GlobalSubscriberNode
        {
            private IGlobalSubscriber m_Subscriber;
            private bool m_IsReleased = false;
            
            public bool IsReleased => m_IsReleased;

            public GlobalSubscriberNode(IGlobalSubscriber subscriber)
            {
                m_Subscriber = subscriber;
            }

            public void Release()
            {
                m_IsReleased = true;
            }

            public void TriggerEventOnSubscriber<TSubscriber>(Action<TSubscriber> action)
                where TSubscriber : IGlobalSubscriber
            {
                if (m_IsReleased)
                {
                    return;
                }
                TSubscriber implementedSubscriber = (TSubscriber)m_Subscriber;
                action.Invoke(implementedSubscriber);
            }

            public bool SubscriberEquals(IGlobalSubscriber subscriber)
            {
                return m_Subscriber == subscriber;
            }
        }
    }
}