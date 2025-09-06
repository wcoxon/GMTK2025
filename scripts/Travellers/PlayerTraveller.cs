using Godot;
using System;

public partial class PlayerTraveller : Traveller
{
    private int health = 300;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health == 0) Player.Instance.OnDeath();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        Money = 100;
        inventory = [50, 15, 210];
    }

    public override void _Process(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;

        if (Player.Instance.State == PlayerState.TRAVEL) travel(simDelta);
    }

    public override void onArrival(Town town)
    {
        base.onArrival(town);

        journey.clearJourney();
        Player.Instance.State = PlayerState.TOWN; // notifies player to enter town state
    }

    public override void AddRumour(Rumour rumour)
    {
        base.AddRumour(rumour);
    }
    public override void RemoveRumour(Rumour rumour)
    {
        base.RemoveRumour(rumour);
        Player.Instance.UI.rumoursUI.removeRumour(rumour);
    }


}
