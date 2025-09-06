using Godot;
using System;

public partial class BanditEncounter : EncounterArea
{

    // encounters have a lifetime, when that expires they are respawned and rehidden
    // but those travellers who hold rumours of it? how might we go about purging knowledge of it
    double timeToRespawn = 12*60;

    public override void _Ready()
    {
        base._Ready();
        respawn();
    }

    public override void OnEncounterEntered(Node3D Body)
    {
        base.OnEncounterEntered(Body);

        Node other = Body.GetParent();

        if (other is PlayerTraveller) Player.Instance.UI.encounterView.DisplayEncounter(this);
        if (other is NPCTraveller npc) robItems(npc);
    }

    public override void chooseOption(int index)
    {
        Show();
        switch (index)
        {
            case 0:
                robItems(Player.Instance.traveller);
                break;
            case 1:
                GD.Print("you run off unharmed");
                break;
        }
    }

    public void robItems(Traveller victim)
    {
        long itemIndex = GD.Randi() % 3;

        victim.inventory[itemIndex] -= Mathf.Min(10, victim.inventory[itemIndex]);
    }

    public override void _PhysicsProcess(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;
        timeToRespawn -= simDelta;

        if (timeToRespawn <= 0) respawn();
    }


    void respawn()
    {
        // pick a new spot
        // move to new spot, 
        // clear/outdate all knowledge of this encounter

        // pick 2 random towns
        // lerp between positions on random factor

        Godot.Collections.Array<Town> test = Player.Instance.World.Towns.Duplicate();
        test.Shuffle();

        Position = test[0].Position.Lerp(test[1].Position, (float)GD.RandRange(0.1f, 0.9f));
    

        GD.Print($" purging knowledge of {Name} rumour..");
        rumour.PurgeKnowledge();

        Hide();

        timeToRespawn = 12*60;

    }
}
