using Godot;
using System;

public partial class DateUI : Label
{
    [Export] DirectionalLight3D sun;
    double hour = 11; // number of ingame hours elapsed

    double hourLength = 3.0; // number of irl seconds for an hour to pass in game (at 1x worldspeed)

    public override void _Process(double delta)
    {
        hour += delta * PlayerView.Instance.worldSpeed / hourLength;

        int day = (int)(hour / 24.0);

        int month = day / 30;
        int year = month / 12;

        Text = $"{(int)(hour - day * 24)}:00  {day - month * 30} - {month - year * 12} - {year}";

        sun.Rotation = Vector3.Right * Mathf.Tau * (float)(hour+6-day) / 24.0f;

    }

}
