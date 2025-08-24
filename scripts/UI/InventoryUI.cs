using Godot;
using System;

public partial class InventoryUI : UIMenu
{
	[Export] VBoxContainer rowsContainer;

	public override string getName() => "Inventory";

	public Traveller Subject
	{
		set
		{
			for (int item = 0; item < 3; item++)
			{
				getItemRow(item).Quantity = value.inventory[item];
			}
		}
	}
	public StockUI getItemRow(int index)
	{
		return rowsContainer.GetChild<StockUI>(index);
	}
	public override void Open()
	{
		base.Open();
		Subject = Player.Instance.traveller;
    }

}
