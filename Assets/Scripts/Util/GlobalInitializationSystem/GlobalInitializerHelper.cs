using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Util.GlobalInitializationSystem
{
    public static class GlobalInitializerHelper
    {
        public static List<IGlobalInitializable> GetInitializablesForSceneType(SceneType sceneType)
        {
            return Assembly.GetAssembly(typeof(IGlobalInitializable)).GetTypes()
                .Where(type => type.GetInterfaces()
                    .Contains(GetInitializableInterfaceForSceneType(sceneType)))
                .Select(type => (IGlobalInitializable) Activator.CreateInstance(type))
                .ToList();
        }

        public static Type GetInitializableInterfaceForSceneType(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.Game:
                    return typeof(IGlobalInitializableInGame);
                case SceneType.MainMenu:
                    return typeof(IGlobalInitializableInMainMenu);
            }
            return null;
        }
    }
}