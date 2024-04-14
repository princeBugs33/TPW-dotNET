namespace Data
{
    public class Ball
    {
        private int _id;
        private double _xPosition;
        private double _yPosition;
        private int _radius;
        private double _xSpeed;
        private double _ySpeed;

        public Ball(int id, double x, double y, int radius, double xSpeed, double ySpeed)
        {
            Id = id;
            XPosition = x;
            YPosition = y;
            Radius = radius;
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

        public int Radius
        {
            get => _radius;
            set => _radius = value;
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