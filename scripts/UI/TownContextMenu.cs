using Godot;
using System;

// i think this could be a good subclass of a ContextMenu class which handles positioning shit
public partial class TownContextMenu : Panel
{
    Town town;

    Label nameLabel;

    public override void _EnterTree()
    {
        base._EnterTree();
        nameLabel = GetNode<Label>("VBoxContainer/Label");
    }

    public void Open(Town _town)
    {
        // use a setter for this instead?

        Show();
        town = _town;
        nameLabel.Text = town.TownName;
    }
    public override void _Process(double delta)
    {
        if (town is null) return;
        var camera = GetViewport().GetCamera3D();
        SetPosition(camera.UnprojectPosition(town.Position), keepOffsets: true);
    }

    public void Inspect()
    {
        Player.Instance.UI.townPanel.Town = town;
        town = null;
        Hide();
    }

    public void Plot()
    {
        // open special plotting UI i think
        // yeah like give this town to the plotting script and close this

        Player.Instance.plotJourney();
        Hide();

    }

}
