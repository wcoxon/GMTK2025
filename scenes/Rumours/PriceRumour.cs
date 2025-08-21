using Godot;
using System;

public partial class PriceRumour : Rumour
{
    
    public Town Town; 
    public int Item; //the good we're talking about- from the goods array in PlayerView.

    //public int rumouredPrice; //what the price was when this rumour was created.

    //public int rumourType; //the good we're talking about- from the goods array in PlayerView.

    //public Town priceAtThisTown; //where the price is from.

    public PriceRumour(Town town, int item) : base()
    {
        Town = town;
        Item = item;
    }
}
