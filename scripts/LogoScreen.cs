using Godot;
using System;

public partial class LogoScreen : TextureRect
{
    [Export] Curve fadeRamp;
    float sample, duration = 2;
    public override void _PhysicsProcess(double delta)
    {
        sample += (float)delta/duration;
        
        
        if (Modulate.A > 0)
        {
            Modulate = new Color(1, 1, 1, fadeRamp.Sample(sample));
        }
        else
        {
            GetParent().RemoveChild(this);
        }
    }
}
