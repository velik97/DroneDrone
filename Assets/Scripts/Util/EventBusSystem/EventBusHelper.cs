using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Util.EventBusSystem
{
    public static class EventBusHelper
    {
        public static List<Type> GetNestedInterfaces(Type majorInterface)
        {
            return Assembly.GetAssembly(majorInterface).GetTypes()
                .Where(type => type.IsInterface && (type.IsSubclassOf(majorInterface) || type == majorInterface))
                .ToList();
        }

        public static List<Type> GetImplementedGlobalSubscribers(IGlobalSubscriber globalSubscriber)
        {
            return globalSubscriber.GetType().GetInterfaces()
                .Where(type => type.GetInterfaces().Contains(typeof(IGlobalSubscriber)))
                .ToList();
        }
    }
}