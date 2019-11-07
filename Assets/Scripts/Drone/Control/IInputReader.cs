namespace Drone.Control
{
    public interface IInputReader
    {
        bool RightIsPressed();
        bool LeftIsPressed();
    }
}