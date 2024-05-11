using Data;

namespace Logic;

public interface IBallController
{
    void GenerateBalls(int number);
    void MoveBalls();
    int Height { get; }
    int Width { get; }
    event NotifyDelegateBallController.NotifyBallController? OnChange;
    List<IBall> GetBalls();
    public void ClearBalls();
}