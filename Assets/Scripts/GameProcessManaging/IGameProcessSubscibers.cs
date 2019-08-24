using Util.EventBusSystem;

namespace GameProcessManaging
{
    public interface IStartGameHandler : IGlobalSubscriber
    {
        void HandleStartGame();
    }
    
    public interface IRestartGameHandler : IGlobalSubscriber
    {
        void HandleRestartGame();
    }

    public interface IGamePauseHandler : IGlobalSubscriber
    {
        void HandlePause();
        void HandleUnPause();
    }
    
    public interface IGameOverHandler : IGlobalSubscriber
    {
        void HandleGameOver();
    }
    
    public interface IGameFinishHandler : IGlobalSubscriber
    {
        void HandleGameFinish();
    }

    public interface IGameOverCountDownHandler : IGlobalSubscriber
    {
        void HandleStartGameOverCountDown();
        void HandleInterruptGameOverCountDown();
        void HandleCompleteGameOverCountDown();
    }
    
    public interface IGameOverCountDownPercentageHandler : IGlobalSubscriber
    {
        void HandleGameOverCountDownPercentageChanged(float percentage);
    }
    
    public interface IGameFinishCountDownHandler : IGlobalSubscriber
    {
        void HandleStartGameFinishCountDown();
        void HandleInterruptGameFinishCountDown();
        void HandleCompleteGameFinishCountDown();
    }
    
    public interface IGameFinishCountDownPercentageHandler : IGlobalSubscriber
    {
        void HandleGameFinishCountDownPercentageChanged(float percentage);
    }
}