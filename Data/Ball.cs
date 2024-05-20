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
        private bool _isMoving = true;
        public event NotifyDelegateBall.NotifyBall? OnChange;

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
        
        public void MoveAsync(Barrier barrier)
        {
            /*
             * Since we are no longer using Task.Run, but lower-level implementation Thread.Start,
             * we lost the ability to benefit from the Task API, which provides advanced monitoring over the threads.
             * To display number of threads, we can use the following powershell command:
             * <code>
             * (Get-Process -Name "View").Threads.Count
             * </code>
             */
            //Task.Run(() => Move(barrier));  // This is the original line responsible for slow thread creation
            new Thread(() => this.Move(barrier)).Start();
            
            
        }
        
        private void Move(Barrier barrier) 
        {
            while (_isMoving)
            {
                OnChange?.Invoke(this);
                barrier.SignalAndWait();
            }
        }
        
        
    }
}