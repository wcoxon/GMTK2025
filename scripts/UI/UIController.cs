using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UIController : CanvasLayer
{
    // acts as an interface for the player script to control and read from UI

    [Export] public TownPanel townPanel;
    [Export] public TimeController timeControlPanel;
    [Export] public InventoryUI inventoryUI;
    [Export] public TradeUI tradeUI;
    [Export] public DialogueUI dialogueUI;
    [Export] public TownContextMenu townContextMenu;
    [Export] public PlottingUI plottingUI;
    [Export] public RumoursUI rumoursUI;



    List<Control> Menus = new();

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            if(Menus.Count>0) CloseUI(Menus.Last());
        }
    }

    public void OpenUI(Control ui)
    {
        if (Menus.Contains(ui)) return;

        ui.Show();
        Menus.Add(ui);
    }
    public void CloseUI(Control ui)
    {
        ui.Hide();
        Menus.Remove(ui);
    }
    public void closeAll()
    {
        while(Menus.Count>0) CloseUI(Menus.Last());
    }

    //public void openRumours()
    //{
    //    // update list in rumoursui
    //    OpenUI(rumoursUI);
    //}
    public void OpenTrade(Town town)
    {
        tradeUI.Town = town; // opening trade menu requires setting its town 
        OpenUI(tradeUI);
    }
    public void OpenInventory(Traveller subject)
    {
        inventoryUI.Subject = subject; // opening inventory requires setting its traveller // well that depends though,, shit idk
        OpenUI(inventoryUI);
    }

    //public void ToggleInventory(Traveller subject)
    //{
    //    inventoryUI.Visible = !inventoryUI.Visible;
    //    if (inventoryUI.Visible) inventoryUI.displayInventory(subject);
    //}
}
