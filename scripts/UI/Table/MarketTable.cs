using Godot;
using System;

// just an easy handle to update market rows from a given town
public partial class MarketTable : Node
{
    [Export] VBoxContainer rowContainer;

    public void updateTable(Town town)
    {
        foreach (MarketUI row in rowContainer.GetChildren()) row.updateRow(town);
    }
}
