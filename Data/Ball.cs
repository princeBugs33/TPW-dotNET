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
        private double _mass;
        private bool _isMoving;
        
        public delegate void Notify(Ball ball);  // delegat do powiadamiania
        public event Notify OnChange;   // zdarzenie

        public Ball(int id, double x, double y, int diameter, double xSpeed, double ySpeed, double mass)
        {
            Id = id;
            XPosition = x;
            YPosition = y;
            Diameter = diameter;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            Mass = mass;
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
        
        public double Mass
        {
            get => _mass;
            set => _mass = value;
        }
        
        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }
        
        public void Move(Barrier barrier) 
        {
            
            // while(_isMoving)
            //OnChange?.Invoke(this);
            // XPosition += XSpeed;
            // YPosition += YSpeed;

            while (true)
            {
                OnChange?.Invoke(this);
                barrier.SignalAndWait();
            }
        }
        
        
    }
}