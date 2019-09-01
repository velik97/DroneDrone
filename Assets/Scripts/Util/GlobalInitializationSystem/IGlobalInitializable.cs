using System;

namespace Util.GlobalInitializationSystem
{
    public interface IGlobalInitializable : IDisposable
    {
        InitializePrior InitializePrior { get; }
        void Initialize();
    }

    public interface IGlobalInitializableInGame : IGlobalInitializable
    {
    }

    public interface IGlobalInitializableInMainMenu : IGlobalInitializable
    {
    }
}