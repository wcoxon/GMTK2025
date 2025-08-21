using System.Collections.Generic;
using Godot;

public partial class NPCTraveller : Traveller
{
    double timeToEmbark = 0;

    //public List<EncounterArea> encounters = new();

    public override void _Ready() => base._Ready();

    public override void _Process(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;

        if (timeToEmbark > 0)
        {
            // still in town, continue countdown
            timeToEmbark -= simDelta;

            // if countdown finished, depart
            if (timeToEmbark <= 0) onDeparture();

        }
        else travel(simDelta);
    }

    public override void onArrival(Town town)
    {
        base.onArrival(town);

        newJourney(); // plan next journey
        timeToEmbark = 30; // start countdown to departure
    }

    void newJourney()
    {
        int currentTownIndex = Player.Instance.World.Towns.IndexOf(Town);

        // pick a town excluding the current one
        long townIndex = GD.Randi() % (Player.Instance.World.Towns.Count - 1);
        if (townIndex >= currentTownIndex) townIndex++;

        Town targetTown = Player.Instance.World.Towns[(int)townIndex];

        // create a journey from current town to randomly picked one
        journey.initJourney(Town, targetTown);
    }


    public void ShowInventory()
    {
        Player.Instance.UI.OpenInventory(this);
    }
    public void HideInventory()
    {
        Player.Instance.UI.CloseInventory();
    }

}
