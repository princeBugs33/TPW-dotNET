using Data;
using Logic;

namespace Model;

public class ModelMain
{
    private IBallController _ballController;
    private IBallRepository _ballRepository;
    public ModelMain(int width, int height)
    {
        _ballRepository = new BallRepository();
        _ballController = new BallController(_ballRepository, width, height);
    }

    public void GenerateBalls(int number)
    {
        _ballController.GenerateBalls(number);
    }
    
    public void MoveBalls()
    {
        _ballController.MoveBalls();
    }
    
    public List<IBall> GetBalls()
    {
        return _ballController.GetBalls();
    }

    public void ClearBalls()
    {
        _ballController.ClearBalls();
    }
}