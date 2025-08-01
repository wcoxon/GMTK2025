using Godot;
using System;

public partial class TerrainArea : EncounterArea
{
    [Export] public float moveSpeedModifier = 0.5f;

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);



        if (Body.GetParent() is not Traveller)
        {
            GD.Print(Body + " entering the Terrain isn't actually a damn Traveller.");
            return;
        }

        Traveller entering = Body.GetParent() as Traveller;

        entering.moveSpeed *= moveSpeedModifier;

    }

    public override void OnEncounterExited(Node3D Body)
    {
        base.OnEncounterExited(Body);

        if (Body.GetParent() is not Traveller)
        {
            GD.Print(Body + " exiting the Terrain isn't actually a damn Traveller.");
            return;
        }

        Traveller entering = Body.GetParent() as Traveller;

        entering.moveSpeed = 1.0f;
    }


}
