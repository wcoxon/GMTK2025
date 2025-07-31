using Godot;
using System;
using System.Collections.Generic;

public partial class TownPanel : Panel
{
    // populates UI contents with information about given town

    [Export] Label nameLabel;
    [Export] Label populationLabel;
    [Export] BoxContainer stockContainer;
    [Export] Button plotButton;
    [Export] Button embarkButton;

    Town target;
    public Town Target
    {
        get => target;
        set
        {
            target = value;

            // update town UI
            nameLabel.Text = target.TownName;
            populationLabel.Text = $"Population: {target.Population}";

            int labelIndex = 0;
            foreach (KeyValuePair<string, int> stockItem in target.Stock)
            {
                stockContainer.GetChild<Label>(labelIndex).Text = $"{stockItem.Key} : {stockItem.Value}";
                labelIndex++;
            }

            // if player isn't on selected town, offer to plot to it
            bool isOnTown = target == PlayerView.instance.player.Town;
            plotButton.Disabled = isOnTown;
        }
    }

    public enum EmbarkMode { Planning, Embarking };
    public EmbarkMode Embarkmode
    {
        set
        {
            switch (value)
            {
                case EmbarkMode.Planning:
                    plotButton.Disabled = true;
                    embarkButton.Disabled = false;
                    break;
                case EmbarkMode.Embarking:
                    plotButton.Disabled = true;
                    embarkButton.Disabled = true;
                    break;
            }
        }
    }
}
