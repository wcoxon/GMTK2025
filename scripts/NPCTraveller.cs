using Godot;
using System;

public partial class NPCTraveller : Traveller
{
    public Town destinationTown;


    public override void _Ready()
    {
        base._Ready();

        
    }




    public void generateNewDestionationTown()
    {

        while (Town == destinationTown)
        {
            int randomItem = (int)GD.Randi() % PlayerView.Instance.allTowns.Count;

            destinationTown = PlayerView.Instance.allTowns[randomItem];
        }
    }
}
