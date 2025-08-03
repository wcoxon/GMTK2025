using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Rumour : Node
{
    public string rumourText; //what the thing says
    public double dayRecieved;
    public double duration; //in whole days.

    public double expirationDay => dayRecieved + duration;

    public Vector3 position; //position to display the rumour, dunno if we need this

    public Rumour(string text, int currentDate, double lifespan, Vector3 encounterPosition)
    {
        rumourText = text;
        dayRecieved = currentDate;
        duration = lifespan;
        position = encounterPosition;
    }
}
