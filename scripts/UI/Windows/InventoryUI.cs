using Godot;
using System;

public partial class InventoryUI : UIWindow
{
	[Export] Label moneyLabel;
	[Export] VBoxContainer rowsContainer;

	public void updateTable(Traveller traveller)
	{
		foreach (StockUI row in rowsContainer.GetChildren()) row.updateRow(traveller);
	}
	
	public void Open(Traveller traveller)
	{
		base.Open();

		moneyLabel.Text = $"Crumbs: {traveller.Money}";
		updateTable(traveller);
	}
	
	// the table logic should be separated from window as well i bet
	public override string getName() => "Inventory";
	public override void Open() => Open(Player.Instance.traveller);
}
