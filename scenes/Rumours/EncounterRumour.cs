using Godot;
using System;

public partial class EncounterRumour : Rumour
{
    public TickBasedEncounter encounterObject; //if the object instance expires before the rumour does it's gonna shit itself so check it's not null

    public EncounterRumour( string text, int currentDate, double lifespan, Vector3 encounterPosition, TickBasedEncounter encounterRef) : base(text,currentDate,lifespan,encounterPosition)
    {
        rumourText = text;
        dayRecieved = currentDate;
        duration = lifespan;
        position = encounterPosition;
        encounterObject = encounterRef;
    }
}
