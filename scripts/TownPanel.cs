using Godot;
using System;
using System.Collections.Generic;

public partial class TownPanel : Panel
{
    // populates UI contents with information about given town

    Town target;
    
    [Export] Label nameLabel;
    [Export] Label populationLabel;
    [Export] BoxContainer stockContainer;

    public Town Target
    {
        get => target;
        set
        {
            target = value;

            //update UI
            nameLabel.Text = target.TownName;
            populationLabel.Text = $"Population: {target.Population}";

            int labelIndex = 0;
            foreach (KeyValuePair<string, int> stockItem in target.Stock)
            {
                stockContainer.GetChild<Label>(labelIndex).Text = $"{stockItem.Key} : {stockItem.Value}";
                labelIndex++;
            }
        }
    }
}
