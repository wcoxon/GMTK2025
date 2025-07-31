using Godot;
using System;
using System.Collections.Generic;

public partial class TownData : Resource
{
    [Export] public string townName;
    [Export] public int population;
    [Export] public Godot.Collections.Dictionary<string, int> stock; // using string bc easier to display but prob change items to enum
}
