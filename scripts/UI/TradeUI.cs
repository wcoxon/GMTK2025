using Godot;
using System;

public partial class TradeUI : Panel
{

	[Export] VBoxContainer productsContainer;
	[Export] Label fundsLabel, costLabel;

	[Export] Button confirmButton;


	Town town;
	public Town Town
	{
		get => town;
		set
		{
			town = value;
			updateUI();
		}
	}

	int totalCost; // dont like this

	public void updateUI()
	{
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);

			row.Stock = (int)Town.Stocks[item];

			int price = Town.appraise((Item)item);
			row.Price = price;
			row.PriceOffset = price - PlayerView.instance.itemBaseValues[item];

			row.Transfer = 0;

			row.PlayerStock = PlayerView.instance.player.inventory[item];
		}

		fundsLabel.Text = $"\nYour Funds: {PlayerView.instance.player.Money} crumbs\nTown Funds: {Town.Wealth} crumbs\n ";

		updateCost(0);
	}

	public void updateCost(float dontuse)
	{
		totalCost = 0;
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);
			totalCost += Town.appraise((Item)item) * row.Transfer;
		}
		costLabel.Text = $"Total Cost: {totalCost} crumbs";

		confirmButton.Disabled = totalCost > PlayerView.instance.player.Money || -totalCost > Town.Wealth; // disable transaction if insufficient funds on either party
	}

	public void confirmTrade()
	{
		// subtract player money, add town money
		PlayerView.instance.player.Money -= totalCost;
		Town.Wealth += totalCost;

		// subtract town stock, add player stock
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);

			Town.Stocks[item] -= row.Transfer;
			PlayerView.instance.player.inventory[item] += row.Transfer;
		}

		// update ui
		updateUI();
	}

	public void closeTrade() => Visible = false;
	
}
