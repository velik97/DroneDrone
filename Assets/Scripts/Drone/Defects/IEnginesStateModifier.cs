namespace Drone.Control
{
    public interface IEnginesStateModifier
    {
        bool ModifiedRightEngineState();
        bool ModifiedLeftEngineState();
    }
}