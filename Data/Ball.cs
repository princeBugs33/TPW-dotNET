namespace Data
{
    public class Ball : IBall
    {
        private int _id;
        private double _xPosition;
        private double _yPosition;
        private int _diameter;
        private double _xSpeed;
        private double _ySpeed;

        public Ball(int id, double x, double y, int diameter, double xSpeed, double ySpeed)
        {
            Id = id;
            XPosition = x;
            YPosition = y;
            Diameter = diameter;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public double XPosition
        {
            get => _xPosition;
            set => _xPosition = value;
        }

        public double YPosition
        {
            get => _yPosition;
            set => _yPosition = value;
        }

        public int Diameter
        {
            get => _diameter;
            set => _diameter = value;
        }

        public double XSpeed
        {
            get => _xSpeed;
            set => _xSpeed = value;
        }

        public double YSpeed
        {
            get => _ySpeed;
            set => _ySpeed = value;
        }
    }
}