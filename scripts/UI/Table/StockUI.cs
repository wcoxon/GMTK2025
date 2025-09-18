using Godot;

// this is really an inventory row
public partial class StockUI : Control
{
	[Export] Label nameLabel, quantityLabel;

	[Export] public int ItemID { get; set; }
	
	public override void _EnterTree()
	{
		base._EnterTree();

		nameLabel.Text = Game.itemNames[ItemID];
	}
	public void updateRow(Traveller traveller)
	{
		quantityLabel.Text = traveller.inventory[ItemID].ToString();
	}
}
