namespace Data;

public class BallRepository : IBallRepository
{
    private List<IBall> _balls = new();

    public void AddBall(IBall ball)
    {
        _balls.Add(ball);
    }

    public void ClearAll()
    {
        _balls.Clear();
    }

    public List<IBall> GetBalls()
    {
        return _balls;
    }
}