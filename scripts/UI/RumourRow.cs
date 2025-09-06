using Godot;
using System;

public partial class RumourRow : Control
{
    public Rumour Rumour {get;set;}

    [Export] public AnimatedSprite2D sourceSprite;

    public void onClick() 
    {
        Player.Instance.UI.closeAll();
        Rumour.focus();
    }
}
