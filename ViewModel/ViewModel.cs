using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Resolvers;
using Data;
using System.Windows.Threading;

namespace ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    

    //public ObservableCollection<Ball> Balls { get; set; }
    public List<Ball> _balls;
    private String _textBoxColor;
    private String _numberOfBalls;
    public ICommand _start { get; }
    public ICommand _stop { get; }
    private bool _isStartEnable;
    private bool _isStopEnable;
    private bool _isTextFieldEnable;
    private DispatcherTimer _timer;

    public ViewModel()
    {
        //_numberOfBalls = "2";
        //_textBoxColor = "Black";
        NumberOfBalls = "2";
        TextBoxColor = "Green";
        _start = new RelayCommand(Start);
        _stop = new RelayCommand(Stop);
        _balls = new();

        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(10); // Set the interval to 10 seconds
        _timer.Tick += Timer_Tick;
        
    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        //UpdateBalls();
        MoveBalls();
        OnPropertyChanged("Balls");
    }

    public void Start()
    {
        IsStartEnable = false;
        IsStopEnable = true;
        IsTextFieldEnable = false;
        UpdateBalls();
        _timer.Start();
    }

    public void Stop()
    {
        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
        _balls.Clear();
        _timer.Stop();
    }
    
    private void MoveBalls()
    {
        foreach (var ball in _balls)
        {
            if (ball.XPosition + ball.XSpeed - ball.Radius <= 0 || ball.XPosition + ball.XSpeed + ball.Radius >= 800)
            {
                ball.XSpeed *= -1;
            }
            if (ball.YPosition + ball.YSpeed - ball.Radius <= 0 || ball.YPosition + ball.YSpeed + ball.Radius >= 600)
            {
                ball.YSpeed *= -1;
            }

            ball.XPosition += ball.XSpeed;
            ball.YPosition += ball.YSpeed;
        }
    }
    
    private void UpdateBalls()
    {
        _balls.Clear();
        Random random = new Random();
        for (int i = 0; i < int.Parse(NumberOfBalls); i++)
        {
            _balls.Add(new Ball(i, 
                random.Next(0, 800), 
                random.Next(0, 600), 
                40,
                random.NextDouble() + random.Next(-2, 2), 
                random.NextDouble() + random.Next(-2, 2)));
            
            
            
        }
        OnPropertyChanged("Balls");
        
    }
    public Ball[]? Balls
    {
        get => _balls.ToArray();
    }
    
    public string TextBoxColor
    {
        get { return _textBoxColor; }
        set
        {
            _textBoxColor = value;
            OnPropertyChanged(nameof(TextBoxColor));
        }
    }
    
    public string NumberOfBalls
    {
        get => _numberOfBalls;
        set
        {
            _numberOfBalls = value;
            //check whether the value is a number and is greater than 0 and less than 100 and only allow start 
            if (int.TryParse(value, out int number) && number > 0 && number < 100)
            {
                IsStartEnable = true;
                TextBoxColor = "Green";
            }
            else
            {
                IsStartEnable = false;
                TextBoxColor = "Red";
            }
            OnPropertyChanged();
        }
    }
    
    

    public bool IsStartEnable
    {
        get => _isStartEnable;
        set
        {
            _isStartEnable = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsStopEnable
    {
        get => _isStopEnable;
        set
        {
            _isStopEnable = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsTextFieldEnable
    {
        get => _isTextFieldEnable;
        set
        {
            _isTextFieldEnable = value;
            OnPropertyChanged();
        }
    }
}