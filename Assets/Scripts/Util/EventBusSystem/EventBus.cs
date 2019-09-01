using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;
using SceneLoading;
using UnityEngine;

namespace Util.EventBusSystem
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<GlobalSubscriberNode>> s_GlobalSubscribers 
            = new Dictionary<Type, List<GlobalSubscriberNode>>();

        private static bool s_IsIterating = false;

        static EventBus()
        {
            SceneLoader.AddWaitPredicate(() => !s_IsIterating);
        }

        public static IDisposable Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> implementedGlobalSubscribers = EventBusHelper.GetImplementedGlobalSubscribers(subscriber);

            bool wasIterating = s_IsIterating;
            s_IsIterating = true;
            
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

            s_IsIterating = wasIterating;
            if (!s_IsIterating)
            {
                CleanUp();
            }

            return new DisposableEventBusSubscriber(subscriber);
        }

        public static void TriggerEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : IGlobalSubscriber
        {
            List<Type> nestedInterfaces = EventBusHelper.GetNestedInterfaces(typeof(TSubscriber));

            bool wasIterating = s_IsIterating;
            s_IsIterating = true;
            
            foreach (Type nestedInterface in nestedInterfaces)
            {
                if (s_GlobalSubscribers.ContainsKey(nestedInterface))
                {
                    foreach (GlobalSubscriberNode globalSubscriber in s_GlobalSubscribers[nestedInterface])
                    {
                        if (!globalSubscriber.IsReleased)
                        {
                            globalSubscriber.TriggerAction(action);
                        }
                    }
                }
            }
            
            s_IsIterating = wasIterating;
            if (!s_IsIterating)
            {
                CleanUp();
            }
        }
        
        private static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> implementedGlobalSubscribers = EventBusHelper.GetImplementedGlobalSubscribers(subscriber);

            foreach (Type interfaceType in implementedGlobalSubscribers)
            {
                if (s_GlobalSubscribers.ContainsKey(interfaceType))
                {
                    List<GlobalSubscriberNode> subscribers = s_GlobalSubscribers[interfaceType];
                    subscribers.FirstOrDefault(node => node.SubscriberEquals(subscriber))?.Release();
                }
            }

            if (!s_IsIterating)
            {
                CleanUp();
            }
        }

        private static void CleanUp()
        {
            foreach (Type type in s_GlobalSubscribers.Keys)
            {
                s_GlobalSubscribers[type].RemoveAll(subscriber => subscriber.IsReleased);
            }

            List<Type> typesToBeRemoved = s_GlobalSubscribers.Keys
                .Where(k => s_GlobalSubscribers[k].Count == 0).ToList();
            
            foreach (Type type in typesToBeRemoved)
            {
                s_GlobalSubscribers.Remove(type);
            }
        }

        private class GlobalSubscriberNode
        {
            private readonly IGlobalSubscriber m_GlobalSubscriber;
            private bool m_IsReleased = false;
            
            public bool IsReleased => m_IsReleased || m_GlobalSubscriber == null;

            public GlobalSubscriberNode(IGlobalSubscriber globalSubscriber)
            {
                m_GlobalSubscriber = globalSubscriber;
            }

            public void Release()
            {
                m_IsReleased = true;
            }

            public bool SubscriberEquals(IGlobalSubscriber globalSubscriber)
            {
                return m_GlobalSubscriber == globalSubscriber;
            }

            public void TriggerAction<TSubscriber>(Action<TSubscriber> action) where TSubscriber : IGlobalSubscriber
            {
                if (IsReleased || !(m_GlobalSubscriber is TSubscriber))
                {
                    return;
                }
                TSubscriber explicitSubscriber = (TSubscriber) m_GlobalSubscriber;
                action(explicitSubscriber);
            }
        }
        
        public class DisposableEventBusSubscriber : IDisposable
        {
            private readonly IGlobalSubscriber m_GlobalSubscriber;

            public DisposableEventBusSubscriber(IGlobalSubscriber globalSubscriber)
            {
                m_GlobalSubscriber = globalSubscriber;
            }

            public void Dispose()
            {
                Unsubscribe(m_GlobalSubscriber);
            }
        }
    }
}