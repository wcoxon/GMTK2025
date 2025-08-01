using Godot;
using System;
using System.Collections.Generic;

public partial class TownData : Resource
{
    [Export] public string townName;
    [Export] public int population;
    [Export] public int wealth;
    [Export] public int[] stocks = new int[3]; // quantities of items
    [Export] public int[] production = new int[3]; // items produced per day?
    [Export] public int[] consumption = new int[3]; // items consumed per day
}
