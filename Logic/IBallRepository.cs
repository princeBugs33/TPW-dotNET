namespace Data;

public interface IBallRepository
{
    void AddBall(IBall? ball); 
    void ClearAll();
    List<IBall> GetBalls();

}