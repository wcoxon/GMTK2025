using Godot;
using System;

public partial class RumourDetector : Area3D
{

    public Traveller m_TravellerSpotter;

    public override void _Ready()
    {
        AreaEntered += OnEncounterDetected;

        m_TravellerSpotter = this.GetParent() as Traveller;
    }



    public void OnEncounterDetected(Area3D area)
    {
        if (area is not TickBasedEncounter)
        {
            return;
        }

        TickBasedEncounter spottedEncounter = area as TickBasedEncounter;

        EncounterContent spottedContent = spottedEncounter.content;

        EncounterRumour newRumour = new(spottedContent.Rumors[0].Text, PlayerView.instance.currentDate, spottedContent.Duration, spottedEncounter.Position, spottedEncounter);

        m_TravellerSpotter.knownRumours.Add(newRumour);

        if (m_TravellerSpotter is PlayerTraveller) //if the player spots the rumour, make it visible
        {
            spottedEncounter.Visible = true;
        }
    }
}
