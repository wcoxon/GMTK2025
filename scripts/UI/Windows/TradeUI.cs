using Godot;
using System;

// trade menu
public partial class TradeUI : UIWindow
{
	[Export] VBoxContainer rowContainer;
	[Export] Label fundsLabel, costLabel;
	[Export] Button confirmButton;
	[Export] AudioStreamPlayer confirmSound;

	private Town town; // is its own window so isn't update by town panel, stores own town thus
	public Town Town
	{
		get => town;
		set
		{
			town = value;
			updateUI(); // for switching towns update
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();

        Hide();

		confirmButton.Pressed += confirmTrade;
	}

	void updateTable()
	{
		foreach (TradeRow row in rowContainer.GetChildren()) row.updateRow(Town);
	}

	public void updateUI()
	{
        barUI.setTitle($"Trade - {Town.TownName}");
		
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
	public override void Open()
	{
		base.Open();

		UIController.Instance.timeControlPanel.Pause();
		UIController.Instance.timeControlPanel.disable();

		resetOffer();
		updateUI();
	}

    public override void Close()
    {
        base.Close();

		UIController.Instance.timeControlPanel.enable();
		UIController.Instance.timeControlPanel.Speed1();
    }

	void resetOffer()
	{
		foreach (TradeRow row in rowContainer.GetChildren())
		{
			row.updateRow(Town); // updates the min and max offer
			row.Offer = 0; // changing the offer should update the arrow colours to be right
		}
	}
}
