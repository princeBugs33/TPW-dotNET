using Data;
using Logic;
using Xunit.Abstractions;

namespace TestLogic;

public class LogicTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public LogicTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void LogicRepositoryTest()
    {
        BallController ballController = new BallController();
        
        ballController.GenerateBalls(10);

        Assert.Equal(10, ballController.GetBalls().Count);

        foreach (var ball in ballController.GetBalls())
        {
            Assert.True(ball.XPosition > 0);
            Assert.True(ball.XPosition < ballController.Width);
            Assert.True(ball.YPosition > 0);
            Assert.True(ball.YPosition < ballController.Height);
        }
    }
    
    // move "test" simulation
    [Fact]
    public void LogicMoveTest()
    {
        BallController ballController = new BallController();
        
        ballController.GenerateBalls(1);

        for (int i = 0; i < 5000; i++)
        {
            ballController.MoveBalls();
            _testOutputHelper.WriteLine(ballController.GetBalls().First().XPosition + " " + ballController.GetBalls().First().YPosition + " " + ballController.GetBalls().First().XSpeed + " " + ballController.GetBalls().First().YSpeed);
        }
    }
}