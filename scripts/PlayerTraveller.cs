using Godot;
using System;

public partial class PlayerTraveller : Traveller
{

    public override void _Process(double delta)
    {


        // travel towards target town // in future this will move them along their path
        travel((float)delta);
    }

    public override void onArrival(Town town)
    {
        // stop from travelling
        // update knowledge of this town
        // enable trading and rumour prompts, oh and like plot journey
        // generally update visuals, ease camera over to town, play sound, and yeah like open some ui
        GD.Print("Entering " + town.TownName);
        PlayerView.instance.PauseWorldSpeed();
    }

    public override void onDeparture()
    {
        // start travelling
        PlayerView.instance.PlayWorldSpeed();
    }
}
