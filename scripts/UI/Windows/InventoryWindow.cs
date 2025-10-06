using Godot;
using System;
using System.Linq;

public partial class InventoryWindow : UIWindow
{
	private Traveller subject;

	[Export] Label moneyLabel;
	[Export] VBoxContainer rowsContainer;

	public override void _EnterTree()
	{
		base._EnterTree();
		
        Hide(); // i hide windows on launch so they can be made visible in editor
	}

	public void updateUI()
	{
		moneyLabel.Text = $"Crumbs: {subject.Money}";
		//foreach (StockUI row in rowsContainer.GetChildren()) row.updateRow(subject);

		// update table

		// well in order to keep it sorted i kind of need to like, uh define the key of the row,
		// i can't do it positionally now because rows change positions

		// i guess i could use the item name to get the item id to get the quantity, but that is bad

		foreach (HBoxContainer row in rowsContainer.GetChildren())
		{
			int itemID = (int)row.GetMeta("ItemID");

			row.GetChild<Label>(0).Text = Game.itemNames[itemID];
			row.GetChild<Label>(1).Text = subject.inventory[itemID].ToString();
		}

	}

	public void OpenInventory(Traveller character)
	{
		subject = character;

		barUI.setTitle($"Inventory - {character.CharacterName}");

		updateUI();
		Open();
	}
	public void OpenPlayerInventory() => OpenInventory(Player.Instance.traveller);
}
