using Godot;
using System;

public partial class menubk : TextureRect
{
    [Export] NoiseTexture2D noise;

    public override void _PhysicsProcess(double delta)
    {
        (noise.Noise as FastNoiseLite).Offset += Vector3.Forward * (float)delta*100.0f;
    }

}
