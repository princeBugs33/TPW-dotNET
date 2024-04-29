using Data;

namespace TestData;

public class BallRepositoryTest
{
    private IBallRepository _ballRepository;
    private double _mass = 1.0;
    
    public BallRepositoryTest()
    {
        _ballRepository = new BallRepository();
        _ballRepository.AddBall(new Ball(1, 5.0, 7.0, 10, 2.0, 3.0, _mass));
        _ballRepository.AddBall(new Ball(1, 5.0, 7.0, 10, 2.0, 3.0, _mass));
    }
    
    [Fact]
    public void GetBallsTest()
    {
        List <IBall> _list = _ballRepository.GetBalls();
        Assert.Equal(2, _list.Count);
    }

    [Fact]
    public void AddBallPositiveTest()
    {
        _ballRepository.AddBall(new Ball(2, 6.0, 4.2, 10, 2.0, 3.0, _mass));
        Assert.Equal(3, _ballRepository.GetBalls().Count);
    }
    
    [Fact]
    public void AddBallNegativeTest()
    {
        _ballRepository.AddBall(null);
        Assert.Equal(2, _ballRepository.GetBalls().Count);
    }
    
    [Fact]
    public void ClearAllTest()
    {
        Assert.Equal(2, _ballRepository.GetBalls().Count);
        _ballRepository.ClearAll();
        Assert.Empty(_ballRepository.GetBalls());
    }
}