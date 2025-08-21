using Godot;
using System.Collections.Generic;

public struct Journey
{
    public Path3D path;
    public PathFollow3D follower;

    public Town destination;

    public Journey(){}

    public void initJourney(Town origin, Town _destination)
    {
        clearJourney();

        destination = _destination;

        path.Curve.AddPoint(origin.Position);
        path.Curve.AddPoint(destination.Position);
    }

    public void clearJourney()
    {
        follower.Progress = 0;
        path.Curve.ClearPoints();
    }
}

[Icon("res://images/burntpizza.jpg")]
public partial class Traveller : Node3D
{
    CollisionShape3D collider;
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
            Position = town?.Position ?? Position;
        }
    }
    SpriteFrames animation;
    [Export] public SpriteFrames Animation { get => animation; set => animation = value; }

    public List<Rumour> knownRumours = new();

    public void AddRumour(Rumour rumour)
    {
        if (knownRumours.Contains(rumour)) return;
        //if (rumour.ThoseWhoKnow.Contains(this)) return;

        knownRumours.Add(rumour);
        rumour.ThoseWhoKnow.Add(this);
    }

    public Journey journey = new();

    public override void _EnterTree()
    {
        GetNode<AnimatedSprite3D>("Sprite").SpriteFrames = Animation; // initialise animated sprite

        journey.path = GetNode<Path3D>("Path3D");
        journey.follower = journey.path.GetNode<PathFollow3D>("PathFollow3D");
        collider = GetNode<CollisionShape3D>("Body/CollisionShape3D");
    }

    public override void _Ready()
    {
        Money = 500;
        inventory[0] = 530;
        inventory[1] = 151;
        inventory[2] = 120;

        onArrival(Town);

        //Player.Instance.TwentyFourTicks += expireRumours;
    }

    public void travel(double simDelta)
    {
        float deltaDistance = moveSpeed * (float)simDelta;
        journey.follower.Progress += deltaDistance;
        Position = journey.follower.Position;

        //Position *= Vector3.Right + Vector3.Back; //Position += Vector3.Up * PlayerView.Instance.world.getMapHeight(Position); // stupid like heightmap walk

        if (journey.follower.ProgressRatio >= 1.0) onArrival(journey.destination);

    }

    virtual public void onArrival(Town town)
    {
        Town = town;
        collider.Disabled = true;
        town.Visitors.Add(this);
    }
    virtual public void onDeparture()
    {
        collider.Disabled = false;
        town.Visitors.Remove(this);
    }

    // call this at the end of every day. Go through your rumours list, remove the stuff that's expired.
    //public void expireRumours()
    //{
    //    if (knownRumours.Count == 0) return;
//
    //    for (int i = knownRumours.Count - 1; i >= 0; i--)
    //    {
    //        //go backwards through the rumours list
    //        if (knownRumours[i].duration == 0) knownRumours.RemoveAt(i);
    //        else knownRumours[i].duration -= 1;
    //    }
    //}
}
