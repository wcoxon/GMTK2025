using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class StormEncounter : MovingEncounter
{
    [Export(PropertyHint.Range, "-0.9,1,0.1")] public float moveSpeedModifier = 0.5f;

    /// we Roll a random float and trigger the event if it's UNDER these values. Higher number = higher chance.
    [Export(PropertyHint.Range, "0,1,0.05")] public float travellerHitChance = 0.6f;
    
    [Export(PropertyHint.Range, "0,1,0.05")] public float townDevastateChance = 0.75f;

    [Export] public Vector3 originCoordinates;

    // we don't want to constantly be devastating towns. We'll do it once a day at most.
    private bool devastatedTownsToday;

    List<Town> townsInsideStorm = [];

    public override void _Ready()
    {
        base._Ready();
        devastatedTownsToday = false;
        PlayerView.instance.EightTicks += DoEveryEightTicks;
        PlayerView.instance.TwentyFourTicks += DoDaily;

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);

        if (Body.GetParent() is not Traveller)
        {
            GD.Print(Body + " entering the storm isn't actually a damn Traveller.");
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
            GD.Print(Body + " exiting the storm isn't actually a damn Traveller.");
            return;
        }

        Traveller entering = Body.GetParent() as Traveller;

        entering.moveSpeed -= moveSpeedModifier;
    }



    public override void DoOnTick()
    {
        base.DoOnTick();

        Array<Node3D> overlappingBodies = GetOverlappingBodies();

        foreach (Node3D Body in overlappingBodies)
        {
            if (Body.GetParent() is not Traveller)
            {
                GD.Print(Body + " inside the storm zone isn't actually a damn Traveller.");
                return;
            }

            Traveller currentTraveller = Body.GetParent() as Traveller;

            if (GD.Randf() > travellerHitChance)
            {
                GD.Print("Storm tried to damage, but lost the coin toss.");
            }
            else
            {
                StormDamageTraveller(currentTraveller);
            }
        }

        if (townsInsideStorm.Count >= 1)
        {
            if (!devastatedTownsToday)
            {
                TryDevastateTown();
            }
        }
    }



    //change direction slightly every eight hours.
    public void DoEveryEightTicks()
    {
        GD.Print("Current Displacement: " + Displacement);
        SlightlyChangeDirection();
        GD.Print("New Displacement: " + Displacement);
    }

    public void DoDaily()
    {
        devastatedTownsToday = false; //we can destroy towns again!!

        if (CheckMaxDistance(originCoordinates))
        {
            GD.Print("Too Far from Origin Coords!");

            Position = originCoordinates;

            GenerateRandomDisplacementVector();
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

    private void TryDevastateTown()
    {
        float randomRoll = GD.Randf();

        GD.Print(randomRoll);
        if (randomRoll > townDevastateChance)
        {
            return;
        }

        devastatedTownsToday = true;

        foreach (Town currentTown in townsInsideStorm)
        {
            DevastateTown(currentTown);
        }

    }

    public void OnAreaEntered(Area3D area)
    {
        if (area.GetParent() is not Town)
        {
            return;
        }

        Town currentTown = area.GetParent() as Town;

        townsInsideStorm.Add(currentTown);
    }

    public void OnAreaExited(Area3D area)
    {
        if (area.GetParent() is not Town)
        {
            return;
        }

        Town currentTown = area.GetParent() as Town;

        townsInsideStorm.Remove(currentTown);
    }

    public void DevastateTown(Town town)
    {
        GD.Print(town + "Was Devastated by storm!");
    }

    /// <summary>
    /// The method for actually resolving a storm damage. Currently empty because players don't have inventories or items to take, or health.
    /// </summary>
    /// <param name="victim">The Traveller to do a bandit attack to</param>
    public void StormDamageTraveller(Traveller victim)
    {
        GD.Print("Storm Damaged!" + victim);
        if (victim is PlayerTraveller)
            PlayerView.instance.notificationManager.AddNotification("Storm damaged you!");
    }

}
