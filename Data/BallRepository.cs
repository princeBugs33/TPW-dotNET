namespace Data;

public class BallRepository
{
    private List<Ball> _balls = new();

    public void AddBall(Ball ball)
    {
        _balls.Add(ball);
    }

    public void ClearAll()
    {
        _balls.Clear();
    }

    public List<Ball> GetBalls()
    {
        return _balls;
    }
}