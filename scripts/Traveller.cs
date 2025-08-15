using Godot;
using System;
using System.Collections.Generic;


struct Journey
{
    List<Node3D> nodes;
    List<Dashes> dashes;
    int index;

    public Journey(List<Node3D> _nodes, List<Dashes> _dashes, int _index)
    {
        nodes = _nodes;
        dashes = _dashes;
        index = _index;
    }
    public Node3D nextWaypoint()
    {
        return nodes[index];
    }
    public void Pop()
    {
        dashes[index].QueueFree();
        index++;
    }
    public void updateDash(Vector3 position)
    {
        var dist_start = position.DistanceTo(dashes[index].LineStart);
        var dist_end = position.DistanceTo(dashes[index].LineEnd);

        dashes[index].SetProgression(dist_start / (dist_start + dist_end));
    }
}

// ok how about yeah, make an activity class, then yeah in, no wait.. just don't call travel if you're not travelling,,

public partial class Traveller : Node3D
{
    int money = 0;
    public int Money { get => money; set => money = value; }

    public int[] inventory = new int[3];
    public float moveSpeed = 1;

    Town town;
    [Export] public Town Town
    {
        get => town;
        set
        {
            town = value;
            Position = town.Position;
        }
    }

    public List<Rumour> knownRumours = new();

    Journey journey;

    public void SetJourney(List<Node3D> nodes, List<Dashes> dashes) => journey = new(nodes, dashes, 0);

    public void travel(float deltaTime)
    {
        deltaTime *= PlayerView.Instance.worldSpeed; // scale delta to simulated time elapsed

        var nextWaypoint = journey.nextWaypoint();
        Vector3 displacement = nextWaypoint.Position - Position;
        float deltaDistance = moveSpeed * deltaTime;

        if (displacement.Length() < deltaDistance)
        {
            Position = nextWaypoint.Position; // this is technically flawed :nerd: you see the remaining progress isn't carried over
            journey.Pop();

            if (nextWaypoint is Town t) onArrival(t);
            else nextWaypoint.QueueFree();
        }
        else
        {
            Translate(displacement.Normalized() * deltaDistance);
            journey.updateDash(Position);
        }
    }

    virtual public void onArrival(Town town) => town.currentTravellers.Add(this);
    virtual public void onDeparture() => town.currentTravellers.Remove(this);
    
    public override void _Ready()
    {
        PlayerView.Instance.TwentyFourTicks += expireRumours;
    }

    // call this at the end of every day. Go through your rumours list, remove the stuff that's expired.
    public void expireRumours()
    {
        if (knownRumours is null) return; // would it ever even be null, not just empty but null?
        if (knownRumours.Count == 0) return;
        
        for (int i = knownRumours.Count - 1; i >= 0; i--)
        {
            //go backwards through the rumours list
            if (knownRumours[i].duration == 0) knownRumours.RemoveAt(i);
            else knownRumours[i].duration -= 1;
        }
    }
}
