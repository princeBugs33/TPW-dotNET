﻿using Data;
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

        int gridWidth = _width / diameterMax + 1;
        int gridHeight = _height / diameterMax + 1;*/

        // fixed ball size
        
        int diameter = 40;

        int gridWidth = _width / diameter + 2;
        int gridHeight = _height / diameter + 2;

        double mass = 2;

        List<(int, int)> coordinates = new List<(int, int)>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
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

            // _ballRepository.AddBall(new Ball(
            //     i,
            //     x,
            //     y,
            //     diameter,
            //     random.NextDouble() + (double)random.Next(-1, 1),
            //     random.NextDouble() + (double)random.Next(-1, 1),
            //     mass)
            // );
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
            Thread.Sleep(10);
        });
        
        foreach (var ball in GetBalls())
        {
             // Task.Run(() => ball.Move(barrier));
             ball.MoveAsync(barrier);
             
        }
    }

    private void MoveBall(IBall ball)
    {
        lock (_lock)
        {
            var newXPosition = ball.XPosition + ball.XSpeed;
            var newYPosition = ball.YPosition + ball.YSpeed;
    
                
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
                    // Oblicz kąt
                    double angle = Math.Atan2(dy, dx);
                
                    // Oblicz prędkości dla każdej z piłek
                    double ballTotalVelocity = Math.Sqrt(ball.XSpeed * ball.XSpeed + ball.YSpeed * ball.YSpeed);
                    double otherBallTotalVelocity =
                        Math.Sqrt(otherBall.XSpeed * otherBall.XSpeed + otherBall.YSpeed * otherBall.YSpeed);
                
                    // Oblicz nowe prędkości uwzględniając masy
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
    
                    // Zaktualizuj prędkości
                    ball.XSpeed = newBallXSpeed;
                    ball.YSpeed = newBallYSpeed;
                    otherBall.XSpeed = newOtherBallXSpeed;
                    otherBall.YSpeed = newOtherBallYSpeed;
                
                    // Napraw nakładanie się kul
                    double overlap = ball.Diameter / 2 + otherBall.Diameter / 2 - distance;
                    newXPosition += overlap * Math.Cos(angle);
                    newYPosition += overlap * Math.Sin(angle);
                }
            }
                            
            if (newYPosition <= 0)
            {
                newYPosition = 0;
                ball.YSpeed *= -1;
            } 
            else if (newYPosition + ball.Diameter >= 600)
            {
                newYPosition = 600 - ball.Diameter;
                ball.YSpeed *= -1;
            }
    
            if (newXPosition <= 0)
            {
                newXPosition = 0;
                ball.XSpeed *= -1;
            } 
            else if (newXPosition + ball.Diameter >= 800)
            {
                newXPosition = 800 - ball.Diameter;
                ball.XSpeed *= -1;
            }
    
            // Przypisz nowe pozycje po zakończeniu wszystkich obliczeń
            ball.XPosition = newXPosition;
            ball.YPosition = newYPosition;
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
        foreach (var ball in GetBalls())
        {
            ball.IsMoving = false;
            ball.OnChange -= MoveBall;
        }
        _ballRepository.ClearAll();
    }
}
