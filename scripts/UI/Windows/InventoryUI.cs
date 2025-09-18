using Godot;
using System;

public partial class InventoryUI : UIWindow
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
		foreach (StockUI row in rowsContainer.GetChildren()) row.updateRow(subject);
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
