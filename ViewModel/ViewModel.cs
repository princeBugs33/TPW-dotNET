using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Resolvers;

namespace ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private String _textBoxColor;
    private String _numberOfBalls;
    public ICommand _start { get; }
    public ICommand _stop { get; }
    private bool _isStartEnable;
    private bool _isStopEnable;
    private bool _isTextFieldEnable;

    public ViewModel()
    {
        //_numberOfBalls = "2";
        //_textBoxColor = "Black";
        NumberOfBalls = "2";
        TextBoxColor = "Green";
        _start = new RelayCommand(Start);
        _stop = new RelayCommand(Stop);

        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
    }

    public void Start()
    {
        IsStartEnable = false;
        IsStopEnable = true;
        IsTextFieldEnable = false;
    }

    public void Stop()
    {
        IsStartEnable = true;
        IsStopEnable = false;
        IsTextFieldEnable = true;
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