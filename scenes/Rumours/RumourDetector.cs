using Godot;
using System;

public partial class RumourDetector : Area3D
{

    public Traveller m_TravellerSpotter;

    public override void _Ready()
    {
        AreaEntered += OnEncounterDetected;

        m_TravellerSpotter = this.GetParent() as Traveller;

        GD.Print(m_TravellerSpotter.Name);
    }



    public void OnEncounterDetected(Area3D area)
    {
        if (area is not TickBasedEncounter)
        {
            return;
        }

        TickBasedEncounter spottedEncounter = area as TickBasedEncounter;

        EncounterContent spottedContent = spottedEncounter.content;

        EncounterRumour newRumour = new(spottedContent.Rumors[0].Text, Player.Instance.currentDate, spottedContent.Duration, spottedEncounter.Position, spottedEncounter);

        GD.Print(newRumour.rumourText);

        GD.Print(newRumour.dayRecieved);

        GD.Print(newRumour.duration);

        GD.Print(newRumour.position);

        GD.Print(newRumour.encounterObject);

        GD.Print(m_TravellerSpotter.Name);


        m_TravellerSpotter.knownRumours.Add(newRumour);
        

        if (m_TravellerSpotter is PlayerTraveller) //if the player spots the rumour, make it visible
        {
            spottedEncounter.Visible = true;
        }
    }
}
