using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class StormEncounter : EncounterArea
{
    [Export] public float moveSpeedModifier;

    Vector3 velocity;

    public override void _Ready()
    {
        base._Ready();
        respawn();
    }

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);

        if (Body.GetParent() is Traveller traveller) traveller.moveSpeed += moveSpeedModifier;

        if (Body.GetParent() is PlayerTraveller) Player.Instance.UI.encounterWindow.DisplayEncounter(this);

    }

    public override void OnEncounterExited(Node3D Body)
    {
        base.OnEncounterExited(Body);

        if (Body.GetParent() is Traveller traveller)
        {
            traveller.moveSpeed -= moveSpeedModifier;
        }
    }
    
    public override void chooseOption(int index) => Show();
    

    public override void _PhysicsProcess(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;
        // move
        Translate(velocity * 0.1f * (float)simDelta);
        // if Position.Length > 50 respawn
        if (Position.Length() > 50) respawn();
    }
    void respawn()
    {
        // pick a new spot
        // move to new spot, 
        // clear/outdate all knowledge of this encounter 

        //  A: for each traveller, for each rumour on them, if about this then remove that rumour

        //  B: observer pattern, 
        //    store rumour, rumour stores knowers of it, rumour has method to wipe itself from its knowers
        
        GD.Print($"Respawning {Name}..");

        // pick 2 random towns
        // lerp between positions on random factor

        float angle = (float)GD.RandRange(0, Mathf.Tau);

        Position = Vector3.Zero;
        velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        GD.Print($" purging knowledge of {Name} rumour..");
        rumour.PurgeKnowledge();

        Hide();

    }
}
