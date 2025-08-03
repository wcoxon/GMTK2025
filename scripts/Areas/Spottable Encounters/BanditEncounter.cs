using Godot;
using Godot.Collections;
using System;
using System.Runtime.Serialization;

public partial class BanditEncounter : TickBasedEncounter
{
    public override void _Ready()
    {
        base._Ready();
        content = EncounterManager.Instance.GetFromClass("bandits")[0];
    }

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

            if (GD.Randf() > content.Chance)
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
        if (victim is PlayerTraveller)
        {
            PlayerView.Instance.encounterView.DisplayEncounter(content);
        }
    }
}
