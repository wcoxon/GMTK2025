using Godot;
using System;

// rumours menu
public partial class RumoursWindow : UIWindow
{
    [Export] PackedScene rumourRowScene;
    VBoxContainer rumoursList;

    public override void _EnterTree()
    {
        base._EnterTree();

        barUI.setTitle("Rumours");
        Hide();

        rumoursList = GetNode<VBoxContainer>("Body/ScrollContainer/RumoursList");
    }


    // called when the player acquires a new rumour from an NPC
    public void AddRumour(Rumour rumour, Traveller source)
    {
        RumourRow rowInstance = rumourRowScene.Instantiate<RumourRow>();

        rowInstance.sourceSprite.SpriteFrames = source.Animation;
        rowInstance.Rumour = rumour;

        rumoursList.AddChild(rowInstance);
    }
    public void removeRumour(Rumour rumour)
    {
        // called when a rumour expires and purges - find the row of this rumour and free it

        foreach (RumourRow row in rumoursList.GetChildren())
        {
            if (row.Rumour != rumour) continue; // skip to matching rumour
            
            row.QueueFree();
            return;
        }
    }

    public void OpenPlayerRumours() => Open();
}
