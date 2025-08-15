using Godot;

public partial class TimeController : Panel
{
    Button button0, button1, button2, button4;
    Label timeDisplay;
    [Export] WorldMap world;

    double hour = 6; // number of ingame hours elapsed
    double hourLength = 60; // number of irl seconds for an hour to pass in game (at 1x worldspeed)

    public override void _Process(double delta)
    {
        hour += delta * PlayerView.Instance.worldSpeed / hourLength;

        double minute = hour % 1.0 * 60;
        int day = (int)(hour / 24.0);
        int month = day / 30;
        int year = month / 12;

        timeDisplay.Text = $"{(int)(hour - day * 24)}:{minute.ToString("00")}  {day - month * 30} - {month - year * 12} - {year}";

        world.Sun.Rotation = Vector3.Right * Mathf.Tau * (float)(hour + 6 - day) / 24.0f;

    }

    public override void _EnterTree()
    {
        base._EnterTree();

        button0 = GetNode<Button>("TimeScales/TimeScale0");
        button1 = GetNode<Button>("TimeScales/TimeScale1");
        button2 = GetNode<Button>("TimeScales/TimeScale2");
        button4 = GetNode<Button>("TimeScales/TimeScale4");

        timeDisplay = GetNode<Label>("DateTime");
    }

    
    public void Pause() => button0.ButtonPressed = true;
    public void Speed1() => button1.ButtonPressed = true;
    public void Speed2() => button2.ButtonPressed = true;
    public void Speed4() => button4.ButtonPressed = true;

    

}
