using System.Reflection;
using Data;
using Logic;

namespace TestLogic;

public class LogicTest
{
    int width = 800;
    int height = 600;
    
    [Fact]
    public void TestGenerateBalls()
    {
        int numberOfBalls = 20;
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);

        ballController.GenerateBalls(numberOfBalls);
        List<IBall> balls = ballController.GetBalls();

        Assert.Equal(numberOfBalls, balls.Count);
        foreach (var ball in balls)
        {
            Assert.True(ball.XPosition >= 0 && ball.XPosition + ball.Diameter <= width);
            Assert.True(ball.YPosition >= 0 && ball.YPosition + ball.Diameter <= height);
            Assert.True(ball.XSpeed is >= -2 and <= 2);
            Assert.True(ball.YSpeed is >= -2 and <= 2);
        }
    }
    
    [Fact]
    public void TestMoveBallsDirection()
    {
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        
        Type tConrtoller = ballController.GetType(); 
        MethodInfo? moveBallMethod = tConrtoller.GetMethod("MoveBall", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        double initialXPosition = ball.XPosition;
        double initialYPosition = ball.YPosition;
        
        moveBallMethod?.Invoke(ballController, [ball]);
        moveBallMethod?.Invoke(ballController, [ball]);

        Assert.True(ball.XPosition != initialXPosition || ball.YPosition != initialYPosition);
    }
    
    [Fact]
    public void TestMoveBallsBoundaryUpperLeftCorner()
    {
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        
        Type tConrtoller = ballController.GetType(); 
        MethodInfo? moveBallMethod = tConrtoller.GetMethod("MoveBall", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        ball.XPosition = 0;
        ball.YPosition = 0;
        ball.XSpeed = -1;
        ball.YSpeed = -1;
        
        moveBallMethod?.Invoke(ballController, [ball]);
        moveBallMethod?.Invoke(ballController, [ball]);

        Assert.True(ball.XPosition > 0);
        Assert.True(ball.YPosition > 0);
        Assert.True(ball.XPosition <= width - ball.Diameter);
        Assert.True(ball.YPosition <= height - ball.Diameter);
    }
    
    [Fact]
    public void TestMoveBallsBoundaryLowerRightCorner()
    {
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        
        Type tConrtoller = ballController.GetType(); 
        MethodInfo? moveBallMethod = tConrtoller.GetMethod("MoveBall", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        ball.XPosition = width - ball.Diameter;
        ball.YPosition = height - ball.Diameter;
        ball.XSpeed = 1;
        ball.YSpeed = 1;
        
        moveBallMethod?.Invoke(ballController, [ball]);
        moveBallMethod?.Invoke(ballController, [ball]);

        Assert.True(ball.XPosition < width - ball.Diameter);
        Assert.True(ball.YPosition < height - ball.Diameter);
        Assert.True(ball.XPosition >= 0);
        Assert.True(ball.YPosition >= 0);
    }
    
    [Fact]
    public void TestClearBalls()
    {
        int numberOfBalls = 5;
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(numberOfBalls);

        ballController.ClearBalls();
        List<IBall> balls = ballController.GetBalls();

        Assert.Empty(balls);
    }
    
    [Fact]
    public void TestGetBalls()
    {
        int numberOfBalls = 5;
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(numberOfBalls);

        List<IBall> balls = ballController.GetBalls();

        Assert.Equal(numberOfBalls, balls.Count);
    }
    
    [Fact]
    public void TestBallXCollision()
    {
        IBall ball1 = new Ball(1, 10.0, 10.0, 10, 1.0, 0, 3.0);
        IBall ball2 = new Ball(2, 22.0 , 10.0, 10, -1.0, 0, 3.0);
        
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        
        Type tConrtoller = ballController.GetType(); 
        MethodInfo? moveBallMethod = tConrtoller.GetMethod("MoveBall", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ballRepository.AddBall(ball1);
        ballRepository.AddBall(ball2);
        
        moveBallMethod?.Invoke(ballController, [ball1]);
        moveBallMethod?.Invoke(ballController, [ball2]);
        
        Assert.Equal(11.0, ball1.XPosition);
        Assert.Equal(21.0, ball2.XPosition);
        
        moveBallMethod?.Invoke(ballController, [ball1]);
        moveBallMethod?.Invoke(ballController, [ball2]);

        Assert.Equal(-1.0, ball1.XSpeed);
        Assert.Equal(1.0, ball2.XSpeed);
    }
    
    [Fact]
    public void TestBallYCollision()
    {
        IBall ball1 = new Ball(1, 20.0, 10.0, 10, 0, 1, 3.0);
        IBall ball2 = new Ball(2, 20.0 , 22.0, 10, 0, -1, 3.0);
        
        IBallRepository ballRepository = new BallRepository();
        IBallController ballController = new BallController(ballRepository, width, height);
        
        Type tConrtoller = ballController.GetType(); 
        MethodInfo? moveBallMethod = tConrtoller.GetMethod("MoveBall", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ballRepository.AddBall(ball1);
        ballRepository.AddBall(ball2);
        
        moveBallMethod?.Invoke(ballController, [ball1]);
        moveBallMethod?.Invoke(ballController, [ball2]);
        
        Assert.Equal(11.0, ball1.YPosition);
        Assert.Equal(21.0, ball2.YPosition);
        
        moveBallMethod?.Invoke(ballController, [ball1]);
        moveBallMethod?.Invoke(ballController, [ball2]);

        Assert.Equal(-1.0, ball1.YSpeed);
        Assert.Equal(1.0, ball2.YSpeed);
    }
}