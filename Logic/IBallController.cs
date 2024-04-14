using Data;

namespace Logic;

public interface IBallController
{
    void GenerateBalls(int number);
    void MoveBalls();
    int Height { get; }
    int Width { get; }
    List<IBall> GetBalls();
    public void ClearBalls();
}