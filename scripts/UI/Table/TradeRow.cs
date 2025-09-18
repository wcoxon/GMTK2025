using Godot;


public partial class TradeRow : Control
{
	[Export] Label nameLabel, stockLabel, playerStockLabel;
	[Export] RichTextLabel priceLabel;
	[Export] SpinBox offerBox;

	[Export] public int ItemID { get; set; }

	public override void _EnterTree()
	{
		base._EnterTree();

		nameLabel.Text = Game.itemNames[ItemID];
	}
	public void updateRow(Town town)
	{
		offerBox.MaxValue = (int)town.Stocks[ItemID];
		stockLabel.Text = offerBox.MaxValue.ToString();

		offerBox.MinValue = -Player.Instance.traveller.inventory[ItemID];
		playerStockLabel.Text = Player.Instance.traveller.inventory[ItemID].ToString();

		int price = town.appraise(ItemID);
		priceLabel.Text = price.ToString();
		
		int priceOffset = price - Game.itemBaseValues[ItemID];
        priceLabel.Text += priceOffset == 0 ? "" : priceOffset < 0 ? $" [color=red]({priceOffset})" : $" [color=green](+{priceOffset})";
	}

	public int Offer { get => (int)offerBox.Value; set => offerBox.Value = value; }
}
