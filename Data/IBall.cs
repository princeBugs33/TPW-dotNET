namespace Data;

public interface IBall
{
    int Id { get; set; }
    double XPosition { get; set; }
    double YPosition { get; set; } 
    int Diameter { get; set; }
    double XSpeed { get; set; }
    double YSpeed { get; set; }
    double Mass { get; set; }
    bool IsMoving { get; set; }
    void Move(Barrier barrier);
    event Ball.Notify OnChange;
}