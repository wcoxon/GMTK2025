using Godot;

// displays stats of an item given a town
public partial class MarketUI : Control
{
    [Export] Label nameLabel, stockLabel;
    [Export] RichTextLabel netProductionLabel, priceLabel;

    [Export] int ItemID { get; set; }

    public override void _EnterTree()
	{
		base._EnterTree();

		nameLabel.Text = Game.itemNames[ItemID];
    }
    public void updateRow(Town town)
    {
        stockLabel.Text = ((int)town.Stocks[ItemID]).ToString();

        int netProduction = (int)(town.Production[ItemID] - town.Consumption[ItemID]);
        netProductionLabel.Text = netProduction == 0 ? "0" : netProduction < 0 ? $"[color=red]{netProduction}" : $"[color=green]+{netProduction}";

        int price = town.appraise(ItemID);
        priceLabel.Text = price.ToString();

        int priceOffset = price - Game.itemBaseValues[ItemID];
        priceLabel.Text += priceOffset == 0 ? "" : priceOffset < 0 ? $" [color=red]({priceOffset})" : $" [color=green](+{priceOffset})";
    }
}
