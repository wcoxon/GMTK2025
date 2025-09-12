using Godot;
using System;
using System.Collections.Generic;

public partial class TownPanel : Control
{
    // populates UI contents with information about given town

    public UIWindow Window;
    // could store tradewindow and make that part of this program but nah i mean
    // single use ig, hmm but then again this is all UI stuff for a town

    [Export] Label nameLabel, populationLabel, wealthLabel;

    [Export] AnimatedSprite2D[] visitorSprites;
    [Export] MarketTable marketTable;

    [Export] Button tradeButton, rumourButton;

    private Town town;
    public Town Town
    {
        get => town;
        set
        {
            town = value;

            Player.Instance.UI.tradeUI.Town = Town;
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Window = GetNode<UIWindow>("Window");
        Window.Hide();
    }
    
    public void focusEntered()
	{
		//MoveToFront();
	}
    public void handleInput(InputEvent evt)
    {
        //if (evt is InputEventMouseButton mb && mb.Pressed)
        //{
        //    MoveToFront();
        //}
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Window.Visible && Town is not null) updateUI(); // keep UI up to date with changing town stats // can't we only update when a stat updates or smth idk like on tick
    }

    public void updateUI()
    {
        //just need to update the dynamic ui to the latest values, town name won't need updating here it's set only when town is selected
        populationLabel.Text = $"Population: {town.Population}";
        wealthLabel.Text = $"Wealth: {town.Wealth}";

        updateVisitors();
        marketTable.updateTable(Town);
    }

    public void updateVisitors()
    {
        for (int i = 0; i < visitorSprites.Length; i++)
        {
            if (i >= Town.Visitors.Count)
            {
                visitorSprites[i].SpriteFrames = null;
                continue;
            }

            visitorSprites[i].SpriteFrames = Town.Visitors[i].Animation;
            visitorSprites[i].Play();
        }
    }

    public void updateActions()
    {
        bool onTown = Player.Instance.State == PlayerState.TOWN;
        bool onSelected = Player.Instance.traveller.Town == Town;

        tradeButton.Disabled = rumourButton.Disabled = !(onTown && onSelected);
    }

    public void Trade() => Player.Instance.UI.tradeUI.Open();
    public void getRumour()
    {
        var shuffledVisitors = Town.Visitors.Duplicate();
        shuffledVisitors.Shuffle();

        foreach (Traveller visitor in shuffledVisitors)
        {
            foreach (Rumour rumour in visitor.knownRumours)
            {
                if (Player.Instance.traveller.knownRumours.Contains(rumour)) continue;

                rumour.reveal(visitor);
                return;
            }
        }
    }


    public void OpenPlayerTown()
    {
        OpenTown(Player.Instance.traveller.Town);
    }

    public void OpenTown(Town town)
    {
        Town = town;
        Window.handleUI.setTitle($"TOWN - {Town.TownName}");
        Window.Open();
        

        nameLabel.Text = Town.TownName; // only update name when town changed
        updateUI(); // update stats UI
        updateActions(); // update buttons
    }

    public void Close()
    {
        Window.Close();
	}

    //public override string getName() => $"Town";
    //public override void Open() => Open(Player.Instance.traveller.Town);
    //
    //public void Open(Town town)
    //{
    //    base.Open();
    //    Town = town;
    //
    //    nameLabel.Text = Town.TownName; // only update name when town changed
    //    updateUI(); // update stats UI
    //    updateActions(); // update buttons
    //
    //    Player.Instance.UI.OpenUI(this); // oops it already did that in base.open though right
    //}
}
