using Godot;
using System;

public partial class TerrainArea : EncounterArea
{
    /// <summary>
    /// additive, so positive number means you move faster, negative means you move slower. I'm pretty sure if it's below -1 shit's gonna break.
    /// </summary>
    [Export(PropertyHint.Range, "-0.9,1,0.1")] public float moveSpeedModifier = -0.5f;

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);



        if (Body.GetParent() is not Traveller)
        {
            //GD.Print(Body + " entering the Terrain isn't actually a damn Traveller.");
            return;
        }

        Traveller entering = Body.GetParent() as Traveller;

        entering.moveSpeed += moveSpeedModifier;

    }

    public override void OnEncounterExited(Node3D Body)
    {
        base.OnEncounterExited(Body);

        if (Body.GetParent() is not Traveller)
        {
            //GD.Print(Body + " exiting the Terrain isn't actually a damn Traveller.");
            return;
        }

        Traveller entering = Body.GetParent() as Traveller;

        entering.moveSpeed -= moveSpeedModifier;
    }


}
