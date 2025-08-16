using Godot;
using System;

public partial class SyncedSprite : AnimatedSprite3D
{
    public override void _PhysicsProcess(double delta)
    {
        SpeedScale = (float)Player.Instance.World.timeScale;
        // this aint great, would rather update speed once with worldspeed or use worldspeed in continuous frame progress
    }
}
