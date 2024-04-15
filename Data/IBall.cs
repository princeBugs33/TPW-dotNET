namespace Data;

public interface IBall
{
    int Id { get; set; }
    double XPosition { get; set; }
    double YPosition { get; set; } 
    int Diameter { get; set; }
    double XSpeed { get; set; }
    public double YSpeed { get; set; }
}