using Godot;
using System;

public enum Item
{
    BROTH,
    PLASTICS,
    EVIL_WATER
}

public partial class StockUI : Panel
{
	[Export] Label nameLabel;
	[Export] Label stockLabel;

	//string[] itemNames = ["Broth", "Plastics", "Evil Water"];

	//public Item Item { set => ItemName = itemNames[(int)value]; }
	public string ItemName { set => nameLabel.Text = value; }
	public int itemQuantity { set => stockLabel.Text = value.ToString(); }
}
