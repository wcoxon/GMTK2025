using Godot;
using System;

public partial class RumoursUI : Control
{
    public UIWindow Window;

    // display rumours in vboxcontainer
    // ideally the giver of the rumour could be shown but for now maybe we should just greybox rly
    [Export] PackedScene rumourRowScene;
    VBoxContainer rumoursList;

    public override void _EnterTree()
    {
        base._EnterTree();

        Window = GetNode<UIWindow>("Window");

        rumoursList = Window.GetNode<VBoxContainer>("RumoursList");
    }


    public void AddRumour(Rumour rumour, Traveller source)
    {
        // called when the player acquires a rumour

        RumourRow rowInstance = rumourRowScene.Instantiate<RumourRow>();

        rowInstance.sourceSprite.SpriteFrames = source.Animation;

        rowInstance.Rumour = rumour;

        rumoursList.AddChild(rowInstance);
    }
    public void removeRumour(Rumour rumour)
    {
        // called when a rumour expires and purges
        // find row of this rumour and free it

        foreach (RumourRow row in rumoursList.GetChildren())
        {
            if (row.Rumour == rumour)
            {
                row.QueueFree();
                break;
            }
        }
    }

    public void OpenPlayerRumours()
    {
        Window.handleUI.setTitle("Rumours");
        Window.Open();
    }
}
