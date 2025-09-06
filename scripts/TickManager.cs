using Godot;
using System;

// just for tick based encounters when that was a thing
public partial class TickManager : Node
{
    private double tick;
    private int eightHourTicker, dayTicker;
    public int currentDate;

    private void OnTick() //increment our other tickers, then set the tick to zero so we can tick again.
    {
        if (eightHourTicker++ >= 8) EmitSignal(SignalName.EightTicks);
        if (dayTicker++ >= 24) EmitSignal(SignalName.TwentyFourTicks);

        tick = 0;
    }
    private void OnEightTicks() => eightHourTicker = 0; //Reset our eight hour tracking variable, so we can check if it hit eight.
    private void OnDay()
    {
        dayTicker = 0; //Reset our day tracking variable, so we can check if it hit twenty four.
        currentDate += 1;
    }

    [Signal] public delegate void TickEventHandler();
    [Signal] public delegate void EightTicksEventHandler();
    [Signal] public delegate void TwentyFourTicksEventHandler();

    public override void _Ready()
    {
        tick = dayTicker = eightHourTicker = currentDate = 0;
        Tick += OnTick;
        EightTicks += OnEightTicks;
        TwentyFourTicks += OnDay;
    }

    //public override void _Process(double delta)
    //{
    //    //tick += delta * Player.Instance.World.timeScale / 60.0;
    //    if (tick >= 1) EmitSignal(SignalName.Tick);
    //}
}
