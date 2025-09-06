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
    [Export] public EncounterView encounterView;

    List<UIWindow> Windows = new();

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Escape") && Windows.Count > 0) CloseUI(Windows.Last());
    }

    public void OpenUI(UIWindow ui)
    {
        if (Windows.Contains(ui)) return;

        ui.Show();
        Windows.Add(ui);
    }
    public void CloseUI(UIWindow ui)
    {
        ui.Hide();
        Windows.Remove(ui);
    }
    public void closeAll()
    {
        while (Windows.Count > 0) CloseUI(Windows.Last());
    }
}
