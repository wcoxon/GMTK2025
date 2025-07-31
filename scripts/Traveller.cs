using Godot;
using System;
using System.Collections.Generic;

public enum Item
{
    BROTH,
    PLASTICS,
    EVIL_WATER
}

public partial class Traveller : Node3D
{
    // generic agent to move between towns
    // behaviour will differ when arriving

    // npc travellers will stay a while to provide rumours to the player about their last village, or maybe any rumour in their recollection newer than a day

    // traders will perform transactions and decide where to go next

    // player will activate state of entering the settlement.

    // a traveller has inventory, money, health, path, recollection?

    int money = 0, health = 5;

    [Export] public Town lastTown;

    List<Node3D> journey_nodes = [];
    List<Dashes> journey_dashes = [];
    int journey_index;

    Path3D path;

    // inventory is items and quantities
    Dictionary<Item, int> inventory = new();

    public void SetJourney(List<Node3D> nodes, List<Dashes> dashes)
    {
        journey_nodes = nodes;
        journey_dashes = dashes;
        journey_index = 0;
    }

    public void travel(float delta)
    {
        if (journey_index >= journey_nodes.Count)
            return;  // No travelling to do.

        var target = journey_nodes[journey_index];
        Vector3 disp = target.Position - Position;

        float travelSpeed = 1;
        float dist = travelSpeed * delta * PlayerView.instance.worldSpeed;

        if (disp.Length() < dist)
        {
            Position = target.Position;
            journey_dashes[journey_index].QueueFree();
            journey_index += 1;
            if (target is Town town) onArrival(town);
            else target.QueueFree();
            return;
        }

        Translate(disp.Normalized() * dist);
        var dist_start = Position.DistanceTo(journey_dashes[journey_index].LineStart);
        var dist_end = Position.DistanceTo(journey_dashes[journey_index].LineEnd);
        journey_dashes[journey_index].SetProgression(dist_start / (dist_start + dist_end));
    }

    virtual public void onArrival(Town town) { }

    virtual public void onDeparture() { }

}
