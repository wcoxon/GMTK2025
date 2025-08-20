using System.Linq;
using Godot;
using Godot.Collections;

public partial class RumorView : Control
{
    [Export] public Town town;

    public static Dictionary<string, Town> towns = [];

    //public override void _Ready()
    //{
    //    // Display any encounter with class testnow immediately.
    //    var contents = EncounterManager.Instance.GetFromClass("testrumor");
    //    if (contents.Any())
    //        DisplayRumor(contents[0].Rumors[0]);
    //}
//
    //public override void _Process(double delta)
    //{
    //    if (Visible)
    //    {
    //        var camera = Player.Instance.GetNode<Camera3D>("Camera3D");
    //        SetPosition(camera.UnprojectPosition(town.Position), keepOffsets: true);
    //    }
    //}
//
    //public void DisplayRumor(EncounterRumorContent rumor)
    //{
    //    Visible = true;
    //    town = towns[rumor.Location];
    //    GetNode<RichTextLabel>("Panel/MarginContainer/RichTextLabel").Text = rumor.Text;
    //}
}
