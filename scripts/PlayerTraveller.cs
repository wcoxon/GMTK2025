using Godot;
using System;

public partial class PlayerTraveller : Traveller
{
    public override void _Ready()
    {
        inventory[(int)Item.BROTH] = 5;
        
        inventory[(int)Item.PLASTICS] = 15;
        
        inventory[(int)Item.EVIL_WATER] = 120;
    }

    public override void _Process(double delta)
    {
        travel((float)delta);
    }

    public override void onArrival(Town town)
    {
        // stop from travelling
        // update knowledge of this town
        // enable trading and rumour prompts, oh and like plot journey
        // generally update visuals, ease camera over to town, play sound, and yeah like open some ui

        Town = town;
        PlayerView.instance.State = GameState.TOWN;
        GD.Print($"{Name} Entering {Town.TownName}");
        PlayerView.instance.PauseWorldSpeed();
    }

    public override void onDeparture()
    {
        PlayerView.instance.State = GameState.TRAVELLING;
        PlayerView.instance.PlayWorldSpeed();
    }
}
