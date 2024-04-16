using Data;

namespace Logic;

public class BallController : IBallController
{
    private IBallRepository _ballRepository;
    private int _width;
    private int _height;
    

    public BallController(IBallRepository ballRepository, int width, int height)
    {
        _ballRepository = ballRepository;
        _width = width;
        _height = height;
    }

    public void GenerateBalls(int number)
    {
        var random = new Random();
        int diameter = 40;
        for (var i = 0; i < number; i++)
        {
            
            var x = random.Next(2) * 2 - 1;
            var y = random.Next(2) * 2 - 1;
            _ballRepository.AddBall(new Ball(
                i, 
                (double)random.Next(0, _width - diameter),
                (double)random.Next(0, _height - diameter), 
                diameter,
                random.NextDouble() + (double)random.Next(-2, 2), 
                random.NextDouble() + (double)random.Next(-2, 2))
            );
        }
    }
    
    public void MoveBalls()
    {
        foreach (var ball in GetBalls())
        {
            var newXPosition = ball.XPosition + ball.XSpeed;
            if (newXPosition <= 0)
            {
                ball.XPosition = 0;
                ball.XSpeed *= -1;
            } 
            else if (newXPosition + ball.Diameter >= 800)
            {
                ball.XPosition = 800 - ball.Diameter;
                ball.XSpeed *= -1;
            }
            else
            {
                ball.XPosition = newXPosition;
            }

            var newYPosition = ball.YPosition + ball.YSpeed;
            if (newYPosition <= 0)
            {
                ball.YPosition = 0;
                ball.YSpeed *= -1;
            } 
            else if (newYPosition + ball.Diameter >= 600)
            {
                ball.YPosition = 600 - ball.Diameter;
                ball.YSpeed *= -1;
            }
            else
            {
                ball.YPosition = newYPosition; 
            }
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