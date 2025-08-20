using Godot;
using System;

public partial class UIController : CanvasLayer
{
    // acts as an interface for the player script to control and read from UI

    [Export] public TownPanel townPanel;
    [Export] public TimeController timeControlPanel;
    [Export] public InventoryUI inventoryUI;
    [Export] public TradeUI tradeUI;



    public void OpenTrade()
    {
        tradeUI.Town = townPanel.Town;
        tradeUI.Visible = true;
    }

    public void ToggleInventory(Traveller subject)
    {
        inventoryUI.Visible = !inventoryUI.Visible;
        if (inventoryUI.Visible) inventoryUI.displayInventory(subject);
    }
}
