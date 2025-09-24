using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// acts as an interface for the player script to control and read from UI
// and like now a manager of the winxp windows system
public partial class UIController : CanvasLayer
{
    public static UIController Instance;

    [Export] public TimeController timeControlPanel;
    [Export] public TownContextMenu townContextMenu;
    [Export] public PlottingUI plottingUI;

    // wins
    [Export] public TownWindow townPanel;
    [Export] public InventoryWindow inventoryUI;
    [Export] public TradeWindow tradeUI;
    [Export] public DialogueWindow dialogueWindow;
    [Export] public RumoursWindow rumoursUI;
    [Export] public EncounterWindow encounterWindow;

    Control WindowsContainer;

    public override void _EnterTree()
    {
        base._EnterTree();

        Instance = this;

        WindowsContainer = GetNode<Control>("Windows");
    }

    // pressing esc closes frontmost window
    public override void _Input(InputEvent @event)
    {
        // esc to close
        if (Input.IsActionJustPressed("Escape") && WindowsContainer.GetChildCount() > 0) WindowsContainer.GetChild<UIWindow>(-1).Close();
    }

    public void OpenUI(UIWindow ui)
    {
        ui.focusWindow();
        ui.Show();
    }
    public void CloseUI(UIWindow ui)
    {
        WindowsContainer.MoveChild(ui, 0);
        ui.Hide();
    }
}
