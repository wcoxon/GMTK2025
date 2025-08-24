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

        // give the player some money and items to start with
        Money = 100;
        inventory[(int)Item.BROTH] = 50;
        inventory[(int)Item.PLASTICS] = 15;
        inventory[(int)Item.EVIL_WATER] = 120;
    }

    public override void _Process(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;

        if (Player.Instance.State == GameState.TRAVEL) travel(simDelta);
    }

    public override void onArrival(Town town)
    {
        base.onArrival(town);

        journey.clearJourney();
        Player.Instance.State = GameState.TOWN; // notifies player to enter town state
    }

    public override void AddRumour(Rumour rumour)
    {
        base.AddRumour(rumour);
        Player.Instance.UI.rumoursUI.AddRumour(rumour);
    }
    public override void RemoveRumour(Rumour rumour)
    {
        base.RemoveRumour(rumour);
        Player.Instance.UI.rumoursUI.removeRumour(rumour);
    }


}
