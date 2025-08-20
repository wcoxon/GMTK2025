using Godot;

// this is really an inventory row
public partial class StockUI : Panel
{
	[Export] Label nameLabel, quantityLabel;
	public string ItemName { set => nameLabel.Text = value; }
	public int Quantity { set => quantityLabel.Text = value.ToString(); }
}
