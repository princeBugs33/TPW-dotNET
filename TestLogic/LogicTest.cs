using Data;
using Logic;
using Xunit.Abstractions;

namespace TestLogic;

public class LogicTest
{
    int width = 800;
    int height = 600;
    
    [Fact]
    public void TestGenerateBalls()
    {
        int numberOfBalls = 5;
        int diameter = 40;
        IBallRepository ballRepository = new BallRepository();
        BallController ballController = new BallController(ballRepository, width, height);

        ballController.GenerateBalls(numberOfBalls);
        List<IBall> balls = ballController.GetBalls();

        Assert.Equal(numberOfBalls, balls.Count);
        foreach (var ball in balls)
        {
            Assert.True(ball.XPosition >= 0 && ball.XPosition <= width - diameter);
            Assert.True(ball.YPosition >= 0 && ball.YPosition <= height - diameter);
            Assert.Equal(diameter, ball.Diameter);
            Assert.True(ball.XSpeed >= -2 && ball.XSpeed <= 2);
            Assert.True(ball.YSpeed >= -2 && ball.YSpeed <= 2);
        }
    }
    
    [Fact]
    public void TestMoveBallsDirection()
    {
        IBallRepository ballRepository = new BallRepository();
        BallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        double initialXPosition = ball.XPosition;
        double initialYPosition = ball.YPosition;
        ballController.MoveBalls();
        ballController.MoveBalls();

        Assert.True(ball.XPosition != initialXPosition || ball.YPosition != initialYPosition);
    }
    
    [Fact]
    public void TestMoveBallsBoundaryUpperLeftCorner()
    {
        IBallRepository ballRepository = new BallRepository();
        BallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        ball.XPosition = 0;
        ball.YPosition = 0;
        ball.XSpeed = -1;
        ball.YSpeed = -1;
        ballController.MoveBalls();
        ballController.MoveBalls();

        Assert.True(ball.XPosition > 0);
        Assert.True(ball.YPosition > 0);
        Assert.True(ball.XPosition <= width - ball.Diameter);
        Assert.True(ball.YPosition <= height - ball.Diameter);
    }
    
    [Fact]
    public void TestMoveBallsBoundaryLowerRightCorner()
    {
        IBallRepository ballRepository = new BallRepository();
        BallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(1);
        IBall ball = ballController.GetBalls()[0];

        ball.XPosition = width - ball.Diameter;
        ball.YPosition = height - ball.Diameter;
        ball.XSpeed = 1;
        ball.YSpeed = 1;
        ballController.MoveBalls();
        ballController.MoveBalls();

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
        BallController ballController = new BallController(ballRepository, width, height);
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
        BallController ballController = new BallController(ballRepository, width, height);
        ballController.GenerateBalls(numberOfBalls);

        List<IBall> balls = ballController.GetBalls();

        Assert.Equal(numberOfBalls, balls.Count);
    }
    
    
    
}