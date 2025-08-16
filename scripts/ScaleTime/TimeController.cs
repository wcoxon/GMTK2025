using Godot;

public partial class TimeController : Panel
{

    Label timeDisplay;
    Button button0, button1, button2, button4, button16, button64;

    public override void _PhysicsProcess(double delta) => timeDisplay.Text = Player.Instance.World.getDateTime();

    public override void _EnterTree()
    {
        timeDisplay = GetNode<Label>("DateTime");

        button0 = GetNode<Button>("TimeScales/TimeScale0");
        button1 = GetNode<Button>("TimeScales/TimeScale1");
        button2 = GetNode<Button>("TimeScales/TimeScale2");
        button4 = GetNode<Button>("TimeScales/TimeScale4");
        button16 = GetNode<Button>("TimeScales/TimeScale16");
        button64 = GetNode<Button>("TimeScales/TimeScale64");
    }
    
    public void Pause() => button0.ButtonPressed = true;
    public void Speed1() => button1.ButtonPressed = true;
    public void Speed2() => button2.ButtonPressed = true;
    public void Speed4() => button4.ButtonPressed = true;
    public void Speed16() => button16.ButtonPressed = true;
    public void Speed64() => button64.ButtonPressed = true;
}
