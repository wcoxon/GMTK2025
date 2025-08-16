using Godot;

public partial class NPCTraveller : Traveller
{
    public override void _Ready() => base._Ready();

    public override void _Process(double delta)
    {
        travel(delta);
    }

    public override void onArrival(Town town)
    {
        Town = town;
        newJourney();
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
}
