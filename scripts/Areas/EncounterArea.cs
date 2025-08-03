using Godot;
using System;
using System.Diagnostics;

public partial class EncounterArea : Area3D
{
    public EncounterContent content;

    public override void _Ready()
    {
        BodyEntered += OnEncounterEntered;

        BodyExited += OnEncounterExited;
    }



    public virtual void OnEncounterEntered(Node3D Body){ }

    public virtual void OnEncounterExited(Node3D Body) { }



}
