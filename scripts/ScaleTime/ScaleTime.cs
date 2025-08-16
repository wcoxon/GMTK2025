using Godot;
using System;

public partial class ScaleTime : Button
{
    [Export] int newTimeScale;

    public override void _Toggled(bool toggledOn)
    {
        if(toggledOn) Player.Instance.setWorldSpeed(newTimeScale);
    }
}
