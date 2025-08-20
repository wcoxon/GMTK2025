using Godot;
using System;

public partial class BanditEncounter : EncounterArea
{

    public override void OnEncounterEntered(Node3D Body)
    {
        Node other = Body.GetParent();

        if (other is PlayerTraveller) Player.Instance.encounterView.DisplayEncounter(this);

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
}
