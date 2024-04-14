using Data;

namespace Logic;

public class BallController : IBallController
{
    private IBallRepository _ballRepository = new BallRepository();
    private int _height = 600;
    private int _width = 800;

    public void GenerateBalls(int number)
    {
        var random = new Random();
        const int radius = 15;
        for (var i = 0; i < number; i++)
        {
            
            var x = random.Next(2) * 2 - 1;
            var y = random.Next(2) * 2 - 1;
            Console.WriteLine(x + " " + y);
            _ballRepository.AddBall(new Ball(i, (double)random.Next(0 + radius, _width - radius),
                (double)random.Next(0 + radius, _height - radius), radius,
                random.NextDouble() + (double)random.Next(-2, 2), random.NextDouble() + (double)random.Next(-2, 2)));
        }
    }

    public void MoveBalls()
    {
        foreach (var ball in GetBalls())
        {
            if (ball.XPosition + ball.XSpeed - ball.Radius <= 0 || ball.XPosition + ball.XSpeed + ball.Radius >= Width)
            {
                ball.XSpeed *= -1;
            }
            if (ball.YPosition + ball.YSpeed - ball.Radius <= 0 || ball.YPosition + ball.YSpeed + ball.Radius >= Height)
            {
                ball.YSpeed *= -1;
            }

            ball.XPosition += ball.XSpeed;
            ball.YPosition += ball.YSpeed;
        }
    }

    public int Height
    {
        get => _height;
    }

    public int Width
    {
        get => _width;
    }
    
    public List<IBall> GetBalls()
    {
        return _ballRepository.GetBalls();
    }

    public void ClearBalls()
    {
        _ballRepository.ClearAll();
    }
}