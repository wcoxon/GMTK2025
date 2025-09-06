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
		PriceOffset = price - Game.itemBaseValues[ItemID];
	}
	//public int Stock
	//{
	//	set
	//	{
	//		stockLabel.Text = value.ToString(); // display quantity
	//		offerBox.MaxValue = value; // limit buy offer to quantity
	//	}
	//}
	//
	//public int PlayerStock
	//{
	//	set
	//	{
	//		playerStockLabel.Text = value.ToString(); // display player quantity
	//		offerBox.MinValue = -value; // limit sell offer to player quantity
	//	}
	//}
	//
	//public int Price { set => priceLabel.Text = value.ToString(); }
	public int PriceOffset
	{
		set
		{
			if (value == 0) return;
			priceLabel.Text += value < 0 ? $" [color=red]({value})[/color]" : $" [color=green](+{value})[/color]";
		}
	}

	public int Offer { get => (int)offerBox.Value; set => offerBox.Value = value; }
}
