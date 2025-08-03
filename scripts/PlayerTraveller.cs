using Godot;
using System;

public partial class PlayerTraveller : Traveller
{
    int health = 1;
    public int Health
    {
        get => health; set
        {
            health = value;
        }
    }

    public override void _Ready()
    {
        Money = 100;

        inventory[(int)Item.BROTH] = 5;
        inventory[(int)Item.PLASTICS] = 15;
        inventory[(int)Item.EVIL_WATER] = 120;
    }

    public override void _Process(double delta)
    {
        travel((float)delta);
        if (health == 0)
            PlayerView.instance.OnDeath();
    }

    public override void onArrival(Town town)
    {
        // stop from travelling
        // update knowledge of this town
        // enable trading and rumour prompts, oh and like plot journey
        // generally update visuals, ease camera over to town, play sound, and yeah like open some ui

        Town = town;
        PlayerView.instance.State = GameState.TOWN; // notifies player to enter town state
        GD.Print($"{Name} Entering {Town.TownName}");
        //PlayerView.instance.PauseWorldSpeed();
    }

    public override void onDeparture()
    {
        //PlayerView.instance.State = GameState.TRAVELLING;
        //PlayerView.instance.PlayWorldSpeed();
    }

    public void GetRumour(Rumour newRumour)
    {
        if (newRumour is EncounterRumour)
        {
            EncounterRumour rumourToAdd = newRumour as EncounterRumour;

            if (rumourToAdd.encounterObject.Visible) // if it's already visible, you already know about it, so whatever.
            {
                return;
            }

            knownRumours.Add(rumourToAdd);
        }
        else if (newRumour is PriceRumour)
        {
            //reveal the stat for the right item I think. this is probably not going to be implemented for the jam.
        }

        

    }
}
