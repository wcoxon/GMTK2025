using Godot;
using System;

public partial class Dashes : MultiMeshInstance3D
{
    [Export] double DASH_LENGTH = 0.5;

    [Export] Mesh mesh;

    public Vector3 LineStart { get; private set; }
    public Vector3 LineEnd { get; private set; }

    public override void _Ready()
    {
        Multimesh = new MultiMesh
        {
            TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
            Mesh = mesh
        };
    }

    public void SetLine(Vector3 start, Vector3 end)
    {
        LineStart = start;
        LineEnd = end;

        var dist = (start - end).Length();
        var n_dashes = (int)Mathf.Floor(dist / DASH_LENGTH);
        var rotation = Mathf.Atan2(start.X - end.X, start.Z - end.Z);
        if (Multimesh.InstanceCount != n_dashes)
            Multimesh.InstanceCount = n_dashes;
        var position = new Transform3D(Basis.Identity, Vector3.Zero).Rotated(Vector3.Right, Mathf.Pi / 2).Rotated(Vector3.Up, rotation);
        for (int i = 0; i < n_dashes; i++)
        {
            Multimesh.SetInstanceTransform(i, position.Translated(end.Lerp(start, (float)(i + 1) / (n_dashes + 1))));
        }
    }

    public void SetProgression(double progression)
    {
        Multimesh.VisibleInstanceCount = (int)(Multimesh.InstanceCount * (1 - progression));
    }
}
