using Godot;
using System;

public partial class PriceRumour : Rumour
{
    public Town Town; 
    public int Item;

    public PriceRumour(Town town, int item) : base()
    {
        Town = town;
        Item = item;
    }
}
