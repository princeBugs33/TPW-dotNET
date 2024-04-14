using System;
using System.Windows;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Model;

public class ModelMain
{
    private Timer _timer;
    
    public void TimerController()
    {
        _timer = new Timer(10);
        _timer.Elapsed += DoMove;
        _timer.AutoReset = true;
        _timer.Enabled = true;
        //_timer.Enable = false;
    }

    private void DoMove(object? sender, ElapsedEventArgs e)
    {
        
    }
}