using Godot;
using System;

public partial class TradeUI : Control
{

	public UIWindow Window;
	public override void _EnterTree()
	{
		base._EnterTree();
		Window = GetNode<UIWindow>("Window");
		Window.Hide();
    }

	[Export] Label fundsLabel, costLabel;
	[Export] Button confirmButton;
	[Export] AudioStreamPlayer confirmSound;

	[Export] VBoxContainer rowContainer;
	private Town town; // is its own window so isn't update by town panel, stores own town thus
	public Town Town
	{
		get => town;
		set
		{
			town = value;
			updateUI();
		}
	}
	void updateTable()
	{
		foreach (TradeRow row in rowContainer.GetChildren()) row.updateRow(Town);
	}

	public void updateUI()
	{
		updateTable();
		updateCost(0);

		fundsLabel.Text = $"\nFunds: {Player.Instance.traveller.Money} crumbs\nTown Funds: {Town.Wealth} crumbs\n";
	}
	public void updateCost(float _)
	{
		int cost = getCost();

		string sign = cost < 0 ? "+" : ""; // you get +2 crumbs, you get -6 crumbs idk how to put it
		costLabel.Text = $"{sign}{-cost} crumbs";


		bool canTraderAfford = Player.Instance.traveller.Money >= cost;
		bool canTownAfford = Town.Wealth >= -cost;

		confirmButton.Disabled = !canTraderAfford || !canTownAfford;
		confirmButton.Text = confirmButton.Disabled ? canTraderAfford ? "insufficient\nTown funds" : "insufficient\nfunds" : "Trade";
	}
	public void confirmTrade()
	{
		int cost = getCost();

		// subtract player money, add town money
		Player.Instance.traveller.Money -= cost;
		Town.Wealth += cost;

		// subtract town stock, add player stock
		foreach (TradeRow row in rowContainer.GetChildren())
		{
			Town.Stocks[row.ItemID] -= row.Offer;
			Player.Instance.traveller.inventory[row.ItemID] += row.Offer;
		}

		confirmSound.Play();

		// update ui
		resetOffer();
		updateUI();
		UIController.Instance.inventoryUI.updateUI(); // prob emit a signal instead of this
	}
	int getCost()
	{
		int sum = 0;
		foreach (TradeRow row in rowContainer.GetChildren())
		{
			sum += Town.appraise(row.ItemID) * row.Offer;
		}
		return sum;
	}
	public void Open()
	{
		Window.Open();

		resetOffer();
		updateUI();

		Player.Instance.UI.timeControlPanel.Pause();
		Player.Instance.UI.timeControlPanel.disable();
	}
	public void Close()
	{
		Window.Close();
		Player.Instance.UI.timeControlPanel.enable();
	}
	
	void resetOffer()
	{
		foreach (TradeRow row in rowContainer.GetChildren())
		{
			row.updateRow(Town); // changes the min and max offer // shit it's still bugged when you first open the window
			row.Offer = 0; // updates the arrows at 0
		}
	}

}
