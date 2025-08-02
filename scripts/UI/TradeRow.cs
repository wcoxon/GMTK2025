using Godot;
using System;

public partial class TradeRow : HBoxContainer
{
	[Export] Label nameLabel;
	[Export] Label stockLabel;
	[Export] RichTextLabel priceLabel;
	[Export] SpinBox transferBox;
	[Export] Label playerStockLabel;

	[Export]
	public string ItemName { get => nameLabel.Text; set => nameLabel.Text = value; }
	public int Stock
	{
		set
		{
			stockLabel.Text = value.ToString();
			transferBox.MaxValue = value;
		}
	}

	public int PlayerStock
	{
		set
		{
			playerStockLabel.Text = value.ToString();
			transferBox.MinValue = -value;
		}
	}

	public int Price { set => priceLabel.Text = value.ToString(); }
	public int PriceOffset
	{
		set
		{
			if (value == 0) return;
            priceLabel.Text += value < 0 ? $" [color=red]({value})[/color]" : $" [color=green](+{value})[/color]";
		}
	}
	public int Transfer { get => (int)transferBox.Value; set => transferBox.Value = value; }


}
