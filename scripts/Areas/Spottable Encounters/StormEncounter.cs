using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class StormEncounter : MovingEncounter
{
    [Export] public float moveSpeedModifier = 0.3f;

    // we don't want to constantly be devastating towns. We'll do it once a day at most.
    //private bool devastatedTownsToday;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);

        if (Body.GetParent() is Traveller traveller) traveller.moveSpeed += moveSpeedModifier;
        
        if (Body.GetParent() is PlayerTraveller) Player.Instance.encounterView.DisplayEncounter(this);

    }

    public override void OnEncounterExited(Node3D Body)
    {
        base.OnEncounterExited(Body);

        if (Body.GetParent() is Traveller traveller)
        {
            traveller.moveSpeed -= moveSpeedModifier;
        }
    }

    private void SlightlyChangeDirection()
    {
        Vector3 newDisplacement = Displacement;

        float xModifier = (float)GD.Randfn(0.0, 0.3);
        float zModifier = (float)GD.Randfn(0.0, 0.3);

        newDisplacement.X += xModifier;

        newDisplacement.Z += zModifier;

        Displacement = newDisplacement;
    }
}
