using Godot;
using System;

public partial class MarketUI : Panel
{
    // shows a town's market information for an item

    [Export] Label nameLabel;
    [Export] Label stockLabel;
    [Export] Label productionLabel;
    [Export] Label consumptionLabel;
    [Export] RichTextLabel priceLabel;


    [Export] public string ItemName { get => nameLabel.Text; set => nameLabel.Text = value; }
    public int Stock { set => stockLabel.Text = value.ToString(); }
    public int Production { set => productionLabel.Text = value.ToString(); }
    public int Consumption { set => consumptionLabel.Text = value.ToString(); }
    public int Price { set => priceLabel.Text = value.ToString(); }
    
    public int PriceOffset {
        set
        {
            if (value != 0) priceLabel.Text += value < 0 ? $" [color=red]({value})[/color]" : $" [color=green](+{value})[/color]"; // this adds a red (-x) or green (+x) to the price label if price is lower or higher than item base value (rn that's 5 for everything but dw)
        }
    }
	
}
