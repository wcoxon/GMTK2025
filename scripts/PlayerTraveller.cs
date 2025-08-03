using Godot;
using System;

public partial class PlayerTraveller : Traveller
{
    public override void _Ready()
    {
        Town = Town;
        Money = 100;

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

        //Town = town;
        PlayerView.Instance.State = GameState.TOWN; // notifies player to enter town state
        //GD.Print($"{Name} Entering {Town.TownName}");
        PlayerView.Instance.Music = Town.Theme;
    }

    public override void onDeparture()
    {
        PlayerView.Instance.Music = null;
    }
}
