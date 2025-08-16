using Godot;
using System;
using System.Collections.Generic;


public struct Journey
{
    public Path3D path;
    public PathFollow3D follower;

    public Town destination;

    public Journey() { }
}

public partial class Traveller : Node3D
{
    public float moveSpeed = 1;
    private int money = 0;
    public int Money { get => money; set => money = value; }

    public int[] inventory = new int[3];

    private Town town;
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

    public Journey journey = new();
    
    public void travel(float deltaTime)
    {
        deltaTime *= (float)Player.Instance.World.timeScale; // scale delta to simulated time elapsed

        float deltaDistance = moveSpeed * deltaTime;

        journey.follower.Progress += deltaDistance;

        Position = journey.follower.Position;

        //Position *= Vector3.Right + Vector3.Back;
        //Position += Vector3.Up * PlayerView.Instance.world.getMapHeight(Position); // stupid like heightmap walk

        if (journey.follower.ProgressRatio >= 1.0) onArrival(journey.destination);

    }

    virtual public void onArrival(Town town) => town.currentTravellers.Add(this);
    virtual public void onDeparture() => town.currentTravellers.Remove(this);

    public override void _EnterTree()
    {
        journey.path = GetNode<Path3D>("Path3D");
        journey.follower = journey.path.GetNode<PathFollow3D>("PathFollow3D");
    }

    public override void _Ready()
    {
        Player.Instance.TwentyFourTicks += expireRumours;
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
