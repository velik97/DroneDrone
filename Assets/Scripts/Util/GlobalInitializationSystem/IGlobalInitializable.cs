using System;

namespace Util.GlobalInitializationSystem
{
    public interface IGlobalInitializable : IDisposable
    {
    }

    public interface IGlobalInitializableInGame : IGlobalInitializable
    {
    }

    public interface IGlobalInitializableInMainMenu : IGlobalInitializable
    {
    }
}