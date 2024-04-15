using Data;

namespace TestData;

public class BallTest
{
    private IBall _ball;
    
    public BallTest()
    {
        _ball = new Ball(1, 5.0, 7.0, 10, 2.0, 3.0);
    }
    
    [Fact]
    public void BallInitializationTest()
    {
        IBall ball = new Ball(1, 5.0, 7.0, 10, 2.0, 3.0);
        
        Assert.Equal(1, ball.Id);
        Assert.Equal(5.0, ball.XPosition);
        Assert.Equal(7.0, ball.YPosition);
        Assert.Equal(10, ball.Diameter);
        Assert.Equal(2.0, ball.XSpeed);
        Assert.Equal(3.0, ball.YSpeed);
    }

    [Fact]
    public void BallIdTest()
    {
        Assert.Equal(1, _ball.Id);
        _ball.Id = 2;
        Assert.Equal(2, _ball.Id);
    }

    [Fact]
    public void BallXYPositionTest()
    {
        Assert.Equal(5.0, _ball.XPosition);
        Assert.Equal(7.0, _ball.YPosition);
        _ball.XPosition = 3.0;
        _ball.YPosition = 3.0;
        Assert.Equal(3.0, _ball.XPosition);
        Assert.Equal(3.0, _ball.YPosition);
    }
    
    [Fact]
    public void BallXYSpeedTest()
    {
        Assert.Equal(2.0, _ball.XSpeed);
        Assert.Equal(3.0, _ball.YSpeed);
        _ball.XSpeed = 4.0;
        _ball.YSpeed = 5.5;
        Assert.Equal(4.0, _ball.XSpeed);
        Assert.Equal(5.5, _ball.YSpeed);
    }
    
    [Fact]
    public void BallRadiusTest()
    {
        Assert.Equal(10, _ball.Diameter);
        _ball.Diameter = 1;
        Assert.Equal(1, _ball.Diameter);
    }
}