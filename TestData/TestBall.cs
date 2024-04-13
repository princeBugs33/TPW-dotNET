using Data;

namespace TestData;

public class TestBall
{
    [Fact]
    public void TestBallInitialization()
    {
        var ball = new Ball(1, 5.0, 7.0, 10, 2.0, 3.0);

        Assert.Equal(1, ball.ID);
        Assert.Equal(5.0, ball.X);
        Assert.Equal(7.0, ball.Y);
        Assert.Equal(10, ball.Radius);
        Assert.Equal(2.0, ball.XSpeed);
        Assert.Equal(3.0, ball.YSpeed);
    }
}