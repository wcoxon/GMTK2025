using Godot;
using System;

public partial class WorldMap : Node3D
{
    public DirectionalLight3D Sun;
    public MeshInstance3D Surface;

    public override void _EnterTree()
    {
        base._EnterTree();

        Sun = GetNode<DirectionalLight3D>("Sun");
        Surface = GetNode<MeshInstance3D>("SurfacePlane");
    }
}
