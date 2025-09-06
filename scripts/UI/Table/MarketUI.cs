using Godot;

// displays stats of a given item and town
public partial class MarketUI : Control
{
    [Export] Label nameLabel, stockLabel, productionLabel, consumptionLabel;
    [Export] RichTextLabel priceLabel;

    [Export] int ItemID { get; set; }

    public override void _EnterTree()
	{
		base._EnterTree();
		nameLabel.Text = Game.itemNames[ItemID];
    }
    public void updateRow(Town town)
    {
        stockLabel.Text = ((int)town.Stocks[ItemID]).ToString();
        productionLabel.Text = ((int)town.Production[ItemID]).ToString();
        consumptionLabel.Text = ((int)town.Consumption[ItemID]).ToString();

        int price = town.appraise(ItemID);
        priceLabel.Text = price.ToString();

        int priceOffset = price - Game.itemBaseValues[ItemID];
        priceLabel.Text += priceOffset == 0 ? "" : priceOffset < 0 ? $" [color=red]({priceOffset})[/color]" : $" [color=green](+{priceOffset})[/color]";
    }
    //public int Stock { set => stockLabel.Text = value.ToString(); }
    //public int Production { set => productionLabel.Text = value.ToString(); }
    //public int Consumption { set => consumptionLabel.Text = value.ToString(); }
    //public int Price { set => priceLabel.Text = value.ToString(); }
    //public int PriceOffset
    //{
    //    set
    //    {
    //        if (value != 0) priceLabel.Text += value < 0 ? $" [color=red]({value})[/color]" : $" [color=green](+{value})[/color]";
    //    }
    //}
}
