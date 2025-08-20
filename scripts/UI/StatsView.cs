using Godot;
using System;

public partial class StatsView : Panel
{
    string format;
    RichTextLabel label;

    public override void _Ready()
    {
        label = GetNode<RichTextLabel>("MarginContainer/RichTextLabel");
        format = label.Text;
    }
    public override void _Process(double delta)
    {
        var player = Player.Instance.traveller;
        label.Text = String.Format(format, player.Money, player.Health);
    }
}
