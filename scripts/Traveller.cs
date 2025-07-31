using Godot;
using System;
using System.Collections.Generic;

public enum Item {
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

    Town target;
    public Town Target
    {
        get => target;
        set { target = value; }
    }

    Path3D path;

    // inventory is items and quantities
    Dictionary<Item, int> inventory = new();

    public void travel(float delta)
    {
        // this is a beeline to the traveller's destination, a stub
        // when we have a Path3D being plotted beforehand, this function will instead offset the progress of a PathFollow3D

        Vector3 disp = target.Position - Position;

        float travelSpeed = 1;
        float dist = travelSpeed * delta * PlayerView.instance.worldSpeed;

        if (disp.Length() < dist)
        {
            Position = target.Position;
            onArrival();
            return;
        }

        Translate(disp.Normalized() * dist);
    }

    virtual public void onArrival() { }
    
    virtual public void onDeparture() {}

}
