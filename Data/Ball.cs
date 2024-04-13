namespace Data
{
    public class Ball
    {
        private int _id;
        private double _x;
        private double _y;
        private int _radius;
        private double _xSpeed;
        private double _ySpeed;

        public Ball(int id, double x, double y, int radius, double xSpeed, double ySpeed)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
            this.Radius = radius;
            this.XSpeed = xSpeed;
            this.YSpeed = ySpeed;
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public double XSpeed
        {
            get { return _xSpeed; }
            set { _xSpeed = value; }
        }

        public double YSpeed
        {
            get { return _ySpeed; }
            set { _ySpeed = value; }
        }
    }
}