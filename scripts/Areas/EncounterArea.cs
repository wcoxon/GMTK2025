using Godot;
using System;
using System.Diagnostics;

public partial class EncounterArea : Area3D
{

    public override void _Ready()
    {
        BodyEntered += OnEncounterEntered;
    }



    public void OnEncounterEntered(Node3D Body)
    {
        GD.Print("doodoo ass");
    }



}
