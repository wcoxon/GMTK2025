using Godot;
using System;

public partial class Toolbar : Panel
{

    [Export] Godot.Collections.Array<UIMenu> Menus;

    Control buttonContainer; // generic but rn it's an HBoxContainer

    public override void _EnterTree()
    {
        base._EnterTree();

        // create buttons for menus

        buttonContainer = GetNode<Control>("ButtonContainer");

        foreach (UIMenu menu in Menus)
        {
            Button menuButton = new Button();
            menuButton.Text = menu.getName();

            menuButton.Connect(Button.SignalName.Pressed, Callable.From(() => openMenu(menu)));

            buttonContainer.AddChild(menuButton);
        }
    }

    public void openMenu(UIMenu menu)
    {
        // the menus open does the stuff the ui controller would be doing differently for each thing but the difference is an override on the thing now
        menu.Open();
    }
    
    //public void openRumours()
    //{
    //    // the rumours UI will list player's known rumours, clicking the rumours will focus on the information in the world
    //    Player.Instance.UI.openRumours();
    //    // ok actually
    //    // toolbar will be generalised, modular, you can add a button and associated UI to open on that button
    //    // instead of specific functions in the UI controller for opening these different things, we have an Open override on the thing that does what needs to be done
    //    // then this script can just go ok open the UI for this button and the UI itself will do the stuff it needs done
    //
    //    // this means we need a base class for these menus/UI panels
    //    // then we can store a list of that on toolbar, and when a button is clicked call open on the corresponding menu
    //
    //
    //}
    //public void openInventory()
    //{
    //    Player.Instance.UI.OpenInventory(Player.Instance.traveller);
    //}
}
