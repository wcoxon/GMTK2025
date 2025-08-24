using Godot;
using System;

public partial class RumourRow : Control
{
    // on click focus on rumour subject in world
    Rumour rumour;
    public Rumour Rumour
    {
        get => rumour;
        set => rumour = value;
    }

    public void onClick()
    {
        // hide UI panels
        // set player target to rumour position

        Player.Instance.UI.closeAll();
        rumour.focus();

    }
}
