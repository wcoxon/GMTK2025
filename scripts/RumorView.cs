using Godot;
using Godot.Collections;

public partial class RumorView : Control
{
    [Export] public Town town;

    public static Dictionary<string, Town> towns = [];

    public override void _Ready()
    {
        // Display any encounter with class testnow immediately.
        foreach (var encounter in EncounterManager.Instance.encounters)
        {
            if (encounter.Class == "testrumor")
            {
                DisplayRumor(encounter.Rumors[0]);
                break;
            }
        }
    }

    public override void _Process(double delta)
    {
        if (Visible)
        {
            var camera = PlayerView.instance.GetNode<Camera3D>("Camera3D");
            SetPosition(camera.UnprojectPosition(town.Position), keepOffsets: true);
        }
    }

    public void DisplayRumor(EncounterManager.EncounterRumorContent rumor)
    {
        Visible = true;
        town = towns[rumor.Location];
        GetNode<RichTextLabel>("Panel/MarginContainer/RichTextLabel").Text = rumor.Text;
    }
}
