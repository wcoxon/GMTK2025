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

    Town town;
    public Town Town
    {
        get => town;
        set
        {
            town = value;

            // update town UI
            nameLabel.Text = town.TownName;

            updateUI();

            // if player isn't on selected town, offer to plot to it
            //bool isOnTown = town == PlayerView.instance.player.Town;
            //tradeButton.Disabled = !isOnTown;
            //rumourButton.Disabled = !isOnTown;
            //plotButton.Disabled = isOnTown;
            updateActions();
        }
    }

    public enum EmbarkMode { Planning, Embarking };
    public EmbarkMode Embarkmode // i wanna use player states or somehow unify them more. 
    {
        set
        {
            switch (value)
            {
                case EmbarkMode.Planning: // transition to plonning state
                    plotButton.Disabled = true;
                    embarkButton.Visible = true;
                    embarkButton.Disabled = false;
                    break;
                case EmbarkMode.Embarking: // transition out of planning state into travelling
                    plotButton.Disabled = true;
                    embarkButton.Disabled = true;
                    embarkButton.Visible = false;
                    break;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Town is not null) updateUI();
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
            marketRow.PriceOffset = price - PlayerView.Instance.itemBaseValues[item];
            
        }
    }

    public void updateActions()
    {
        bool onTown = PlayerView.Instance.State == GameState.TOWN;
        bool onSelected = PlayerView.Instance.player.Town == Town; // gotta be careful, travellers town is still last town whilst travelling, but you shouldn't be able to trade whilst travelling

        
        tradeButton.Disabled = !(onTown && onSelected); // show trade if on selected town and in town state
        rumourButton.Disabled = !(onTown && onSelected); // show rumour if on selected town and in town state
        plotButton.Disabled = !(onTown && !onSelected); // show plan if off selected town and in town state
    }
}
