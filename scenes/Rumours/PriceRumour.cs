using Godot;
using System;

public partial class PriceRumour : Rumour
{
    public int rumouredPrice; //what the price was when this rumour was created.

    public int rumourType; //the good we're talking about- from the goods array in PlayerView.

    public Town priceAtThisTown; //where the price is from.

    public PriceRumour(string text, int currentDate, int lifespan, Vector3 encounterPosition, int price, int stockType, Town town) : base(text, currentDate, lifespan, encounterPosition)
    {
        rumourText = text;
        dayRecieved = currentDate;
        duration = lifespan;
        position = encounterPosition;
        rumouredPrice = price;
        rumourType = stockType;
        priceAtThisTown = town;
    }
}
