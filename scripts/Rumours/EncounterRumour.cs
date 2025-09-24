using Godot;
using System;

public partial class EncounterRumour : Rumour
{
    public EncounterArea Encounter;

    public EncounterRumour(EncounterArea encounter) : base()
    {
        Encounter = encounter;
    }
    public override void focus()
    {
        base.focus();
        Player.Instance.moveTo(Encounter.Position);
    }

    public override void reveal(Traveller revealer)
    {
        base.reveal(revealer);

        Encounter.Show();
        Player.Instance.UI.dialogueWindow.revealEncounter(revealer, Encounter);
    }

}
