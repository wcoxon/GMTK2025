using Godot;
using System;

public partial class InventoryUI : Panel
{
    [Export] VBoxContainer rowsContainer;

    public StockUI getItemRow(int index)
    {
        return rowsContainer.GetChild<StockUI>(index);
    }

	public void displayInventory(Traveller traveller)
	{
		for (int item = 0; item < 3; item++)
		{
			getItemRow(item).Quantity = traveller.inventory[item];
		}
	}
	
}
