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
        BallController ballController = new BallController(800, 600);
        
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
}