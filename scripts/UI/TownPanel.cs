using Godot;
using System;
using System.Collections.Generic;

public partial class TownPanel : UIMenu
{
    // populates UI contents with information about given town

    [Export] Label nameLabel;
    [Export] Label populationLabel;
    [Export] Label wealthLabel;
    [Export] BoxContainer stockContainer;

    [Export] AnimatedSprite2D[] visitorSprites;

    [Export] Button tradeButton;
    [Export] Button rumourButton;
    [Export] Button plotButton;
    [Export] Button embarkButton;
    [Export] Button cancelButton;

    Town town;
    public Town Town
    {
        get => town;
        set
        {
            town = value;

            Open();
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Town is not null) updateUI(); // keep UI up to date with changing town stats
    }

    public void updateUI() // update info specifically, could rename to that tbf
    {
        //just need to update the dynamic ui to the latest values, town name won't need updating here it's set only when town is selected
        populationLabel.Text = $"Population: {town.Population}";
        wealthLabel.Text = $"Wealth: {town.Wealth}";

        for (int item = 0; item < 3; item++)
        {
            MarketUI marketRow = stockContainer.GetChild<MarketUI>(item);
            int price = town.appraise((Item)item);


            marketRow.Stock = (int)town.Stocks[item];
            marketRow.Production = (int)town.Production[item];
            marketRow.Consumption = (int)town.Consumption[item];
            marketRow.Price = price;
            marketRow.PriceOffset = price - Player.Instance.itemBaseValues[item];

        }

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
        bool onTown = Player.Instance.State == GameState.TOWN;
        bool onSelected = Player.Instance.traveller.Town == Town; // gotta be careful, travellers town is still last town whilst travelling, but you shouldn't be able to trade whilst travelling

        tradeButton.Disabled = !(onTown && onSelected); // show trade if on selected town and in town state
        rumourButton.Disabled = !(onTown && onSelected); // show rumour if on selected town and in town state
        plotButton.Disabled = !(onTown && !onSelected); // show plan if off selected town and in town state
    }

    public void Trade() => Player.Instance.UI.OpenTrade(Town);
    public void getRumour()
    {
        // pick someone in the town
        // pick a rumour/encounter to reveal
        // show dialogue and remove rumour from npc

        Godot.Collections.Array<Traveller> shuffledVisitors = Town.Visitors.Duplicate();
        shuffledVisitors.Shuffle();

        foreach (Traveller visitor in shuffledVisitors)
        {
            //GD.Print($"visitor: {visitor.Name}");
            foreach (Rumour rumour in visitor.knownRumours)
            {
                if (Player.Instance.traveller.knownRumours.Contains(rumour)) continue;
                rumour.reveal(visitor);
                //visitor.knownRumours.Remove(rumour); // maybe purge knowledge of it,, rn you hear the same rumour from ppl who know it
                //actually just don't show rumours the player knows, purging wouldn't prevent hearing about again after the purge

                return;
            }
        }
    }
    
    public override void Open()
    {
        nameLabel.Text = Town.TownName; // only update name when town changed
        updateUI(); // update stats UI
        updateActions(); // update buttons

        Player.Instance.UI.OpenUI(this);
    }
    public override void Close()
    {
        Player.Instance.UI.CloseUI(this);
    }
}
