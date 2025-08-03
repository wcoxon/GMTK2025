using Godot;
using System;

public partial class NPCTraveller : Traveller
{
    public Town destinationTown;





    public void generateNewDestionationTown()
    {
        

        while (Town == destinationTown)
        {
            int randomItem = (int)GD.Randi() % PlayerView.instance.allTowns.Count;
        }
    }
}
