using Godot;
using Godot.Collections;
using System;

public partial class BanditEncounter : TickBasedEncounter
{
    [Export] public float encounterPercentage = 0.5f;


    public override void DoOnTick()
    {
        base.DoOnTick();

        Array<Node3D> overlappingBodies = GetOverlappingBodies();

        foreach (Node3D Body in overlappingBodies)
        {
            if (Body.GetParent() is not Traveller)
            {
                GD.Print(Body + " inside the Bandit zone isn't actually a damn Traveller.");
                return;
            }

            Traveller currentTraveller = Body.GetParent() as Traveller;

            if (GD.Randf() > encounterPercentage)
            {
                BanditAttack(currentTraveller);
            }
            else
            {
                GD.Print("Bandits tried to attack, but lost the coin toss.");
            }
        }


    }

    /// <summary>
    /// The method for actually resolving a bandit attack. Currently empty because players don't have inventories or items to take, or health.
    /// </summary>
    /// <param name="victim">The Traveller to do a bandit attack to</param>
    public void BanditAttack(Traveller victim)
    {
        GD.Print("Bandits! They Got You!!!" + victim);
    }


}
