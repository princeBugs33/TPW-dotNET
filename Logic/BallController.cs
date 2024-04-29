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

        // variable ball size
        /*int diameterMin = 20;
        int diameterMax = 40;

        int gridWidth = _width / diameterMax + 1;
        int gridHeight = _height / diameterMax + 1;*/

        // fixed ball size
        int diameter = 40;

        int gridWidth = _width / diameter + 1;
        int gridHeight = _height / diameter + 1;

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

            _ballRepository.AddBall(new Ball(
                i,
                x,
                y,
                diameter,
                random.NextDouble() + (double)random.Next(-1, 1),
                random.NextDouble() + (double)random.Next(-1, 1),
                mass)
            );
        }
    }
    
    public void MoveBalls()
    {
        
        foreach (var ball in GetBalls())
        {
            var newXPosition = ball.XPosition + ball.XSpeed;
            var newYPosition = ball.YPosition + ball.YSpeed;
            
            ////////
            // Pierwsza implemntacja kolizji, problem z nakładaniem się kul
            //
            // foreach (var otherBall in GetBalls())
            // {
            //     if (ball.Id == otherBall.Id)
            //     {
            //         continue;
            //     }
            //     double distance = Math.Sqrt(Math.Pow(newXPosition - otherBall.XPosition, 2) + Math.Pow(newYPosition - otherBall.YPosition, 2));
            //     if (distance <= ball.Diameter)
            //     {
            //         double overlap = ball.Diameter - distance;
            //         
            //         if(ball.XSpeed > 0)
            //         {
            //             newXPosition -= overlap;
            //         }
            //         else
            //         {
            //             newXPosition += overlap;
            //         }
            //         
            //         if(ball.YSpeed > 0)
            //         {
            //             newYPosition -= overlap;
            //         }
            //         else
            //         {
            //             newYPosition += overlap;
            //         }
            //         
            //         
            //
            //         // newXPosition -= overlap ;
            //         // newYPosition -= overlap ;
            //         // otherBall.XPosition += overlap / 2;
            //         // otherBall.YPosition += overlap / 2;
            //
            //         var tempXSpeed = ball.XSpeed;
            //         var tempYSpeed = ball.YSpeed;
            //         ball.XSpeed = otherBall.XSpeed;
            //         ball.YSpeed = otherBall.YSpeed;
            //         otherBall.XSpeed = tempXSpeed;
            //         otherBall.YSpeed = tempYSpeed;
            //         
            //         
            //         //ball.XSpeed *= -1;
            //         //ball.YSpeed *= -1;
            //     }
            // }
            
            //////
            // Druga implementacja kolizji, działa najlepiej, nie uwzględnia masy
            //
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
                    double otherBallTotalVelocity = Math.Sqrt(otherBall.XSpeed * otherBall.XSpeed + otherBall.YSpeed * otherBall.YSpeed);
            
                    // Oblicz nowe prędkości
                    double newBallXSpeed = ballTotalVelocity * Math.Cos(angle);
                    double newBallYSpeed = ballTotalVelocity * Math.Sin(angle);
                    double newOtherBallXSpeed = otherBallTotalVelocity * Math.Cos(angle + Math.PI);
                    double newOtherBallYSpeed = otherBallTotalVelocity * Math.Sin(angle + Math.PI);
            
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
            
            //////
            // Trzecia implementacja kolizji, uwzględnia masę, problem z blokującymi się kulami (chyba fixed??)
            //
            // foreach (var otherBall in GetBalls())
            // {
            //     if (ball.Id == otherBall.Id)
            //     {
            //         continue;
            //     }
            //
            //     double dx = newXPosition - otherBall.XPosition;
            //     double dy = newYPosition - otherBall.YPosition;
            //     double distance = Math.Sqrt(dx * dx + dy * dy);
            //
            //     if (distance < ball.Diameter / 2 + otherBall.Diameter / 2)
            //     {
            //         // Oblicz kąt
            //         double angle = Math.Atan2(dy, dx);
            //
            //         // Oblicz prędkości dla każdej z piłek
            //         double ballTotalVelocity = Math.Sqrt(ball.XSpeed * ball.XSpeed + ball.YSpeed * ball.YSpeed);
            //         double otherBallTotalVelocity =
            //             Math.Sqrt(otherBall.XSpeed * otherBall.XSpeed + otherBall.YSpeed * otherBall.YSpeed);
            //
            //         // Oblicz nowe prędkości uwzględniając masy
            //         double newBallXSpeed, newBallYSpeed, newOtherBallXSpeed, newOtherBallYSpeed;
            //         if (ball.Mass == otherBall.Mass)
            //         {
            //             // Jeśli masy są identyczne, prędkości są zamieniane miejscami
            //             newBallXSpeed = otherBall.XSpeed;
            //             newBallYSpeed = otherBall.YSpeed;
            //             newOtherBallXSpeed = ball.XSpeed;
            //             newOtherBallYSpeed = ball.YSpeed;
            //             Console.WriteLine("Masy są równe");
            //         }
            //         else
            //         {
            //             Console.WriteLine("Masy nie są równe");
            //             newBallXSpeed =
            //                 (ballTotalVelocity * (ball.Mass - otherBall.Mass) +
            //                  2 * otherBall.Mass * otherBallTotalVelocity) / (ball.Mass + otherBall.Mass) *
            //                 Math.Cos(angle);
            //             newBallYSpeed =
            //                 (ballTotalVelocity * (ball.Mass - otherBall.Mass) +
            //                  2 * otherBall.Mass * otherBallTotalVelocity) / (ball.Mass + otherBall.Mass) *
            //                 Math.Sin(angle);
            //             newOtherBallXSpeed =
            //                 (otherBallTotalVelocity * (otherBall.Mass - ball.Mass) +
            //                  2 * ball.Mass * ballTotalVelocity) / (ball.Mass + otherBall.Mass) *
            //                 Math.Cos(angle + Math.PI);
            //             newOtherBallYSpeed =
            //                 (otherBallTotalVelocity * (otherBall.Mass - ball.Mass) +
            //                  2 * ball.Mass * ballTotalVelocity) / (ball.Mass + otherBall.Mass) *
            //                 Math.Sin(angle + Math.PI);
            //         }
            //
            //         // Zaktualizuj prędkości
            //         ball.XSpeed = newBallXSpeed;
            //         ball.YSpeed = newBallYSpeed;
            //         otherBall.XSpeed = newOtherBallXSpeed;
            //         otherBall.YSpeed = newOtherBallYSpeed;
            //
            //         // Napraw nakładanie się kul
            //         double overlap = ball.Diameter / 2 + otherBall.Diameter / 2 - distance;
            //         newXPosition += overlap * Math.Cos(angle);
            //         newYPosition += overlap * Math.Sin(angle);
            //     }
            // }

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
        _ballRepository.ClearAll();
    }
}
///////////////////////////////////////////
// Stan z Etapu 1
//
// using Data;
//
// namespace Logic;
//
// public class BallController : IBallController
// {
//     private IBallRepository _ballRepository;
//     private int _width;
//     private int _height;
//     
//
//     public BallController(IBallRepository ballRepository, int width, int height)
//     {
//         _ballRepository = ballRepository;
//         _width = width;
//         _height = height;
//     }
//
//     public void GenerateBalls(int number)
//     {
//         var random = new Random();
//         int diameter = 40;
//         for (var i = 0; i < number; i++)
//         {
//             
//             var x = random.Next(2) * 2 - 1;
//             var y = random.Next(2) * 2 - 1;
//             _ballRepository.AddBall(new Ball(
//                 i, 
//                 (double)random.Next(0, _width - diameter),
//                 (double)random.Next(0, _height - diameter), 
//                 diameter,
//                 random.NextDouble() + (double)random.Next(-2, 2), 
//                 random.NextDouble() + (double)random.Next(-2, 2))
//             );
//         }
//     }
//     
//     public void MoveBalls()
//     {
//         foreach (var ball in GetBalls())
//         {
//             var newXPosition = ball.XPosition + ball.XSpeed;
//             if (newXPosition <= 0)
//             {
//                 ball.XPosition = 0;
//                 ball.XSpeed *= -1;
//             } 
//             else if (newXPosition + ball.Diameter >= 800)
//             {
//                 ball.XPosition = 800 - ball.Diameter;
//                 ball.XSpeed *= -1;
//             }
//             else
//             {
//                 ball.XPosition = newXPosition;
//             }
//
//             var newYPosition = ball.YPosition + ball.YSpeed;
//             if (newYPosition <= 0)
//             {
//                 ball.YPosition = 0;
//                 ball.YSpeed *= -1;
//             } 
//             else if (newYPosition + ball.Diameter >= 600)
//             {
//                 ball.YPosition = 600 - ball.Diameter;
//                 ball.YSpeed *= -1;
//             }
//             else
//             {
//                 ball.YPosition = newYPosition; 
//             }
//         }
//     }
//
//     public int Height
//     {
//         get => _height;
//     }
//
//     public int Width
//     {
//         get => _width;
//     }
//     
//     public List<IBall> GetBalls()
//     {
//         return _ballRepository.GetBalls();
//     }
//
//     public void ClearBalls()
//     {
//         _ballRepository.ClearAll();
//     }
//}