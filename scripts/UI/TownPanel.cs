using Godot;
using System;
using System.Collections.Generic;

public partial class TownPanel : Panel
{
    // populates UI contents with information about given town

    [Export] Label nameLabel;
    [Export] Label populationLabel;
    [Export] Label wealthLabel;
    [Export] BoxContainer stockContainer;
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

            nameLabel.Text = town.TownName; // only update name when town changed

            updateUI(); // update stats UI
            updateActions(); // update buttons
        }
    }

    public void plan()
    {
        plotButton.Disabled = true;

        embarkButton.Visible = true;
        cancelButton.Visible = true;

        embarkButton.Disabled = false;
        cancelButton.Disabled = false;
    }
    public void embark()
    {
        embarkButton.Disabled = true;
        cancelButton.Disabled = true;

        embarkButton.Visible = false;
        cancelButton.Visible = false;

        if (Town is not null) updateActions(); // i mean if town is null we should just like, hide ts anyway
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Town is not null) updateUI(); // continuously update displayed stats
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
    }

    public void updateActions()
    {
        bool onTown = Player.Instance.State == GameState.TOWN;
        bool onSelected = Player.Instance.traveller.Town == Town; // gotta be careful, travellers town is still last town whilst travelling, but you shouldn't be able to trade whilst travelling

        tradeButton.Disabled = !(onTown && onSelected); // show trade if on selected town and in town state
        rumourButton.Disabled = !(onTown && onSelected); // show rumour if on selected town and in town state
        plotButton.Disabled = !(onTown && !onSelected); // show plan if off selected town and in town state
    }
}
