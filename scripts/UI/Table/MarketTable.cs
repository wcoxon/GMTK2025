using Godot;
using System;

// just an easy handle to update market rows from a given town
public partial class MarketTable : Node
{
    [Export] VBoxContainer rowContainer;

    //private Town town;
    //public Town Town
    //{
    //    get => town;
    //    set
    //    {
    //        town = value;
    //        updateTable();
    //    }
    //}

    public void updateTable(Town town)
    {
        foreach (MarketUI row in rowContainer.GetChildren()) row.updateRow(town);
    }
}
