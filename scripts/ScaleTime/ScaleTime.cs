using Godot;
using System;

public partial class ScaleTime : Button
{
    /// Take a number and set the world timescale in player view to it, that class handles all the important shit.

    [Export] public int newTimeScale;

    public override void _Toggled(bool toggledOn)
    {
        if(toggledOn) PlayerView.Instance.setWorldSpeed(newTimeScale);
    }
}
