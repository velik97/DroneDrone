using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.EventBusSystem
{
    public class EventBusCleaner
    {
        private Dictionary<Type, List<EventBus.GlobalSubscriberNode>> m_GlobalSubscribers;
        private object m_Lock;
        private float m_TimeBetweenCleanUps;

        public EventBusCleaner(Dictionary<Type, List<EventBus.GlobalSubscriberNode>> globalSubscribers, object @lock, float timeBetweenCleanUps)
        {
            m_GlobalSubscribers = globalSubscribers;
            m_Lock = @lock;
            m_TimeBetweenCleanUps = timeBetweenCleanUps;
            
            CoroutineHandler.StartCoroutineOnHandler(CleanUpCoroutine());
        }

        private IEnumerator CleanUpCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(m_TimeBetweenCleanUps);
                CleanUpAsync();
            }
        }

        private void CleanUpAsync()
        {
            AsyncTaskManager.CallFuncParallel(CleanUp);
        }

        private void CleanUp()
        {
            lock (m_Lock)
            {
                List<Type> typesToBeCleaned = new List<Type>();
                foreach (Type type in m_GlobalSubscribers.Keys)
                {
                    List<EventBus.GlobalSubscriberNode> nodesToBeCleaned = new List<EventBus.GlobalSubscriberNode>();
                    foreach (EventBus.GlobalSubscriberNode node in m_GlobalSubscribers[type])
                    {
                        if (node.IsReleased)
                        {
                            nodesToBeCleaned.Add(node);
                        }
                    }

                    foreach (EventBus.GlobalSubscriberNode node in nodesToBeCleaned)
                    {
                        m_GlobalSubscribers[type].Remove(node);
                    }

                    if (m_GlobalSubscribers[type].Count == 0)
                    {
                        typesToBeCleaned.Add(type);
                    }
                }

                foreach (Type type in typesToBeCleaned)
                {
                    m_GlobalSubscribers.Remove(type);
                }
            }
        }
    }
}