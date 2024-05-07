using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Resolvers;
using Data;
using System.Windows.Threading;
using Model;

namespace ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    

    //public ObservableCollection<Ball> Balls { get; set; }
    private String _textBoxColor;
    private ModelMain _modelMain;
    private String _numberOfBalls;
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }
    private bool _isStartEnable;
    private bool _isStopEnable;
    private bool _isTextFieldEnable;
    private DispatcherTimer _timer;
    private int _width = 800;
    private int _height = 600;
    

    public ViewModel()
    {
        TextBoxColor = "Green";
        StartCommand = new RelayCommand(Start, () => IsStartEnable);
        StopCommand = new RelayCommand(Stop, () => IsStopEnable);
        _modelMain = new ModelMain(_width, _height);
        _numberOfBalls = "5";

        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Tick += Timer_Tick;
        
    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        //_modelMain.MoveBalls();
        OnPropertyChanged("Balls");
    }

    public void Start()
    {
        IsStartEnable = false;
        IsStopEnable = true;
        IsTextFieldEnable = false;
        _modelMain.GenerateBalls(int.Parse(NumberOfBalls));
        // Task.Run(() => _modelMain.MoveBalls());
        _modelMain.MoveBalls();
        _timer.Start();
    }

    public void Stop()
    {
        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
        _timer.Stop();
        _modelMain.ClearBalls();
        OnPropertyChanged("Balls");
    }
    
    public IBall[]? Balls
    {
        get => _modelMain.GetBalls().ToArray();
    }
    
    public string TextBoxColor
    {
        get => _textBoxColor;
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