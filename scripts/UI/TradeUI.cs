using Godot;
using System;

public partial class TradeUI : Panel
{
	[Export] AudioStreamPlayer confirmSound;
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
			row.PriceOffset = price - Player.Instance.itemBaseValues[item];

			row.Transfer = 0;

			row.PlayerStock = Player.Instance.traveller.inventory[item];
		}

		fundsLabel.Text = $"\nYour Funds: {Player.Instance.traveller.Money} crumbs\nTown Funds: {Town.Wealth} crumbs\n ";

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

		confirmButton.Disabled = totalCost > Player.Instance.traveller.Money || -totalCost > Town.Wealth; // disable transaction if insufficient funds on either party
	}

	public void confirmTrade()
	{
		// subtract player money, add town money
		Player.Instance.traveller.Money -= totalCost;
		Town.Wealth += totalCost;

		// subtract town stock, add player stock
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);

			Town.Stocks[item] -= row.Transfer;
			Player.Instance.traveller.inventory[item] += row.Transfer;
		}

		confirmSound.Play();

		// update ui
		updateUI();
	}

	public void closeTrade() => Visible = false;
	
}
