using Godot;
using System;

public partial class InventoryUI : Panel
{
	[Export] VBoxContainer stocksContainer;

	public StockUI getStockUI(int index)
	{
		return stocksContainer.GetChild<StockUI>(index);
	}

	public void displayInventory(Traveller traveller)
	{
		for (int item = 0; item < 3; item++) // 3 being number of item types, sry for magic number
		{
			getStockUI(item).Item = (Item)item;
			getStockUI(item).itemQuantity = traveller.inventory[item];
		}
	}
	
}
