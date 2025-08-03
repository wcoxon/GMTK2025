using Godot;
using System;
using System.Collections.Generic;

public partial class Traveller : Node3D
{
    // generic agent to move between towns
    // behaviour will differ when arriving
    // npc travellers will stay a while to provide rumours to the player about their last village, or maybe any rumour in their recollection newer than a day
    // traders will perform transactions and decide where to go next
    // player will activate state of entering the settlement.
    // a traveller has inventory, money, health, path, recollection?

    int money = 0;
    public int Money { get => money; set => money = value; }

    //[Export] public Godot.Collections.Dictionary<Item, int> inventory;
    public int[] inventory = new int[3];

    public float moveSpeed = 1;

    Town town;
    [Export]
    public Town Town
    {
        get => town;
        set
        {
            town = value;
            Position = town.Position;
        }
    }

    List<Node3D> journey_nodes = [];
    List<Dashes> journey_dashes = [];
    int journey_index;

    //Path3D path; // we could use built in path logic if we so desire
    //Dictionary<Item, int> inventory = new(); // implement later :p

    public void SetJourney(List<Node3D> nodes, List<Dashes> dashes)
    {
        journey_nodes = nodes;
        journey_dashes = dashes;
        journey_index = 0;
    }

    public void travel(float delta)
    {
        if (journey_index >= journey_nodes.Count) return;  // No travelling to do. 

        var nextWaypoint = journey_nodes[journey_index];
        Vector3 disp = nextWaypoint.Position - Position;

        float dist = moveSpeed * delta * PlayerView.instance.worldSpeed;

        if (disp.Length() < dist)
        {
            Position = nextWaypoint.Position;
            journey_dashes[journey_index].QueueFree();
            journey_index += 1;

            if (nextWaypoint is Town town) onArrival(town); // enter town
            else nextWaypoint.QueueFree();

            return;
        }

        Translate(disp.Normalized() * dist);
        var dist_start = Position.DistanceTo(journey_dashes[journey_index].LineStart);
        var dist_end = Position.DistanceTo(journey_dashes[journey_index].LineEnd);
        journey_dashes[journey_index].SetProgression(dist_start / (dist_start + dist_end));
    }

    virtual public void onArrival(Town town) { }

    virtual public void onDeparture() { }

    public void ResetJourney()
    {
        while (journey_index < journey_nodes.Count)
        {
            if (journey_nodes[journey_index] is Town town) { }
            else journey_nodes[journey_index].QueueFree();
            journey_dashes[journey_index].QueueFree();
            journey_index++;
        }          
    }
}
