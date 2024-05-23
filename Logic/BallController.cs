using Data;
using System.Diagnostics;

namespace Logic;

public class BallController : IBallController
{
    private IBallRepository _ballRepository;
    private int _width;
    private int _height;
    private readonly object _lock = new object();
    public event NotifyDelegateBallController.NotifyBallController? OnChange;
    
    public BallController(IBallRepository ballRepository, int width, int height)
    {
        _ballRepository = ballRepository;
        _width = width;
        _height = height;
    }

    public void GenerateBalls(int number)
    {
        var random = new Random();

        // variable ball size
        /*int diameterMin = 20;
        int diameterMax = 40;

        int gridWidth = (_width - diameterMax)  / diameterMax;
        int gridHeight = (_height - diameterMax) / diameterMax;*/

        // fixed ball size
        
        int diameter = 40;
        
        int gridWidth = (_width - diameter) / diameter;
        int gridHeight = (_height - diameter) / diameter;

        double mass = 2;

        List<(int, int)> coordinates = new List<(int, int)>();

        for (int x = 0; x <= gridWidth; x++)
        {
            for (int y = 0; y <= gridHeight; y++)
            {
                coordinates.Add((x, y));
            }
        }

        coordinates = coordinates.OrderBy(x => Random.Shared.Next()).ToList();

        for (var i = 0; i < number; i++)
        {
            var (gridX, gridY) = coordinates[i];
            


            // fixed ball size
            double x = gridX * diameter;
            double y = gridY * diameter;

            // variable ball size
            /*int diameter = random.Next(diameterMin, diameterMax + 1);
            double x = gridX * diameterMax + random.NextDouble() * (diameterMax - diameter);
            double y = gridY * diameterMax + random.NextDouble() * (diameterMax - diameter);*/

            Ball ball = new Ball(
                i,
                x,
                y,
                diameter,
                random.NextDouble() + (double)random.Next(-1, 1),
                random.NextDouble() + (double)random.Next(-1, 1),
                mass
            );
            
            _ballRepository.AddBall(ball);
            ball.OnChange += MoveBall;
        }
    }
    
    public void MoveBalls()
    {
        
        Barrier barrier = new Barrier(GetBalls().Count, (b) =>
        {
            OnChange?.Invoke();
            Thread.Sleep(20);
        });
        
        foreach (var ball in GetBalls())
        {
             ball.MoveAsync(barrier); // Task.Run(() => ball.Move(barrier));
        }
    }

    private void MoveBall(IBall ball)
    {
        //lock (_lock)
        //{
            var newXPosition = ball.XPosition + ball.XSpeed;
            var newYPosition = ball.YPosition + ball.YSpeed;
            // Variable for proper collision physics
            bool collisionAlreadyHandled = false;
                
            foreach (var otherBall in GetBalls())
            {
                if (ball.Id == otherBall.Id)
                {
                    continue;
                }
                
                double dx = newXPosition - otherBall.XPosition;
                double dy = newYPosition - otherBall.YPosition;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                
                if (distance < ball.Diameter / 2 + otherBall.Diameter / 2)
                {
                    // Calculate the angle
                    double angle = Math.Atan2(dy, dx);
                
                    // Calculate the speed of the balls
                    double ballTotalVelocity = Math.Sqrt(ball.XSpeed * ball.XSpeed + ball.YSpeed * ball.YSpeed);
                    double otherBallTotalVelocity = Math.Sqrt(otherBall.XSpeed * otherBall.XSpeed + otherBall.YSpeed * otherBall.YSpeed);
                
                    // Calculate the speed of the balls taking into account the masses
                    double newBallXSpeed =
                        (ballTotalVelocity * (ball.Mass - otherBall.Mass) +
                         2 * otherBall.Mass * otherBallTotalVelocity) / (ball.Mass + otherBall.Mass) *
                        Math.Cos(angle);
                    double newBallYSpeed =
                        (ballTotalVelocity * (ball.Mass - otherBall.Mass) +
                         2 * otherBall.Mass * otherBallTotalVelocity) / (ball.Mass + otherBall.Mass) *
                        Math.Sin(angle);
                    double newOtherBallXSpeed =
                        (otherBallTotalVelocity * (otherBall.Mass - ball.Mass) +
                         2 * ball.Mass * ballTotalVelocity) / (ball.Mass + otherBall.Mass) *
                        Math.Cos(angle + Math.PI);
                    double newOtherBallYSpeed =
                        (otherBallTotalVelocity * (otherBall.Mass - ball.Mass) +
                         2 * ball.Mass * ballTotalVelocity) / (ball.Mass + otherBall.Mass) *
                        Math.Sin(angle + Math.PI);
    
                    // Required for protection against making a move twice for ball that detected collision first
                    double oldBallXSpeed = ball.XSpeed;
                    double oldBallYSpeed = ball.YSpeed;
                    
                    // Update speed
                    ball.XSpeed = newBallXSpeed;
                    ball.YSpeed = newBallYSpeed;
                    otherBall.XSpeed = newOtherBallXSpeed;
                    otherBall.YSpeed = newOtherBallYSpeed;
                
                    // Protection against making a move twice for ball that detected collision first
                    if (oldBallXSpeed != ball.XSpeed && oldBallYSpeed != ball.YSpeed)
                    {
                        double overlap = ball.Diameter / 2 + otherBall.Diameter / 2 - distance;
                        newXPosition += overlap * Math.Cos(angle);
                        newYPosition += overlap * Math.Sin(angle);
                        
                    }
                    else
                    {
                        collisionAlreadyHandled = true;
                    }
                    
                    // double overlap = ball.Diameter / 2 + otherBall.Diameter / 2 - distance;
                    // newXPosition += overlap * Math.Cos(angle);
                    // newYPosition += overlap * Math.Sin(angle);
                }
            }
                 
            // Collision with the wall
            if (newYPosition <= 0)
            {
                newYPosition = 0;
                ball.YSpeed *= -1;
            } 
            else if (newYPosition + ball.Diameter >= _height)
            {
                newYPosition = _height - ball.Diameter;
                ball.YSpeed *= -1;
            }
    
            if (newXPosition <= 0)
            {
                newXPosition = 0;
                ball.XSpeed *= -1;
            } 
            else if (newXPosition + ball.Diameter >= _width)
            {
                newXPosition = _width - ball.Diameter;
                ball.XSpeed *= -1;
            }
            
            // Assigning a value after all calculations
            if(collisionAlreadyHandled == false)
            {
                ball.XPosition = newXPosition;
                ball.YPosition = newYPosition;
            }
            
            // ball.XPosition = newXPosition;
            // ball.YPosition = newYPosition;
        //}
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
        foreach (var ball in GetBalls())
        {
            ball.IsMoving = false;
            ball.OnChange -= MoveBall;
        }
        _ballRepository.ClearAll();
    }
}
