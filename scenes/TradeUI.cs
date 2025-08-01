using Godot;
using System;

public partial class TradeUI : Panel
{

	[Export] VBoxContainer productsContainer;
	[Export] Label fundsLabel;
	[Export] Label costLabel;

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
	int totalCost;


	public void updateUI()
	{
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);

			row.Stock = Town.Stocks[item];
			row.PlayerStock = PlayerView.instance.player.inventory[item];

			row.Price = Town.appraise((Item)item);

			row.Quantity = 0;
		}

		fundsLabel.Text = $"\nYour Funds: {PlayerView.instance.player.Money} crumbs\nTown Funds: {Town.Wealth} crumbs\n ";

		totalCost = 0;
		costLabel.Text = $"Total Cost: {totalCost} crumbs";
	}

	public void updateCost(float dontuse)
	{
		totalCost = 0;
		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);
			totalCost += Town.appraise((Item)item) * row.Quantity;
		}
		costLabel.Text = $"Total Cost: {totalCost} crumbs";

		confirmButton.Disabled = totalCost > PlayerView.instance.player.Money || -totalCost > Town.Wealth;
		
	}

	public void confirmTrade()
	{
		// subtract player money
		// add town money

		PlayerView.instance.player.Money -= totalCost;
		Town.Wealth += totalCost;


		// subtract town stock
		// add player stock

		for (int item = 0; item < 3; item++)
		{
			TradeRow row = productsContainer.GetChild<TradeRow>(item);

			Town.Stocks[item] -= row.Quantity;
			PlayerView.instance.player.inventory[item] += row.Quantity;
		}

		// update ui
		updateUI();
	}

	public void closeTrade()
	{
		Visible = false;
	}
}
