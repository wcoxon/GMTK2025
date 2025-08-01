using Godot;
using System;

public partial class TradeRow : HBoxContainer
{
	[Export] Label itemName;
	[Export] Label itemStock;
	[Export] Label itemPrice;
	[Export] SpinBox tradeAmount;

	[Export]
	public string ItemName
	{
		get => itemName.Text;
		set
		{
			itemName.Text = value;
		}
	}

	public int PlayerStock
	{
		set
		{
			tradeAmount.MinValue = -value;
		}
	}

	public int Stock
	{
		set
		{
			itemStock.Text = value.ToString();
			tradeAmount.MaxValue = value;
		}
	}
	public int Price { set => itemPrice.Text = value.ToString(); }
	
	public int Quantity
	{
		get => (int)tradeAmount.Value;
		set
		{
			tradeAmount.Value = value;
		}
	}


}
