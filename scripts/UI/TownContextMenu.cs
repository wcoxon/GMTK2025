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

        town = _town;
        nameLabel.Text = town.TownName;

        GetNode<Button>("VBoxContainer/Button2").Disabled = town == Player.Instance.traveller.Town;

        Show();
    }
    public override void _Process(double delta)
    {
        if (town is null) return;
        var camera = GetViewport().GetCamera3D();
        SetPosition(camera.UnprojectPosition(town.Position), keepOffsets: true);
    }

    public void Inspect()
    {
        Player.Instance.UI.townPanel.OpenTown(town);
        Close();
    }

    public void Plot()
    {
        Player.Instance.plotJourney();
        Close();
    }

    void Close()
    {
        town = null;
        Hide();
    }
}
