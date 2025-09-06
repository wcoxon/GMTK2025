using System.Collections.Generic;
using Godot;

public partial class NPCTraveller : Traveller
{
    double timeToEmbark = 0;

    public override void _Ready() => base._Ready();

    public override void _Process(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;

        if (timeToEmbark > 0)
        {
            // still in town, continue countdown
            timeToEmbark -= simDelta;

            // if countdown finished, depart
            if (timeToEmbark <= 0) onDeparture();

        }
        else travel(simDelta);
    }

    public override void onDeparture()
    {
        base.onDeparture();

        chooseDestination();
    }


    public override void onArrival(Town town)
    {
        base.onArrival(town);

        Trade();
        //chooseDestination();
        //newJourney(); // plan next journey
        timeToEmbark = 30; // start countdown to departure
    }

    void newJourney()
    {
        int currentTownIndex = Player.Instance.World.Towns.IndexOf(Town);

        // pick a town excluding the current one
        long townIndex = GD.Randi() % (Player.Instance.World.Towns.Count - 1);
        if (townIndex >= currentTownIndex) townIndex++;

        Town targetTown = Player.Instance.World.Towns[(int)townIndex];

        // create a journey from current town to randomly picked one
        journey.initJourney(Town, targetTown);
    }

    void Trade()
    {
        /// sell what town consumes
        /// buy as much as you can of what town produces
        /// 
        /// 
        /// 


        //int sellPrice = 0;
        int buyPrice = 0;

        for (int item = 0; item < 3; item++)
        {
            if (Town.netProduction(item) < 0)
            {
                // sell
                //sellPrice += Town.appraise(item) * inventory[item];

                int sellAmount = inventory[item];

                GD.Print($"sold {sellAmount} {Game.itemNames[item]}");

                inventory[item] -= sellAmount;
                Town.Stocks[item] += sellAmount;
                Money += Town.appraise(item) * sellAmount;

            }
            else
            {
                // buy
                buyPrice += Town.appraise(item) * (int)Town.Stocks[item];
            }
        }

        // now we will sell all our shit regardless here,

        //Money += sellPrice;


        // but we will now buy as much as we can too
        // by taking how much of the buy price we can afford with our money,
        // so if i have 1/2 of buy price, then i can buy half the items ig

        // i guess floor how much i buy though?

        float buyPortion = Mathf.Min(Money / (float)buyPrice,1);

        for (int item = 0; item < 3; item++)
        {
            if (Town.netProduction(item) < 0) continue;

            int buyAmount = (int)(buyPortion * Town.Stocks[item]);

            GD.Print($"bought {buyAmount} {Game.itemNames[item]}");

            Town.Stocks[item] -= buyAmount;
            inventory[item] += buyAmount;
            Money -= buyAmount * Town.appraise(item);

        }


    }

    void Trade1()
    {
        // get how much i can sell for (of things the town consumes)
        // get how much town can sell for (of things the town produces)

        // whatever is less is the max value we can exchange

        // scale the other down to that value and trade these

        int townExportsPrice = 0;
        int townImportsPrice = 0;

        for (int itemID = 0; itemID < 3; itemID++)
        {
            if (Town.netProduction(itemID) <= 0)
            {
                // consumed, imported item
                // how much the trader has * appraisal
                townImportsPrice += inventory[itemID] * Town.appraise(itemID);
            }
            else
            {
                // produced, exported item
                // how much the town has * appraisal
                townExportsPrice += (int)Town.Stocks[itemID] * Town.appraise(itemID);
            }
        }


        // whatever is less is the max value we can exchange

        // so if the exports is less 
        //  then we buy all the exports
        //  in exchange for exportprice/importprice scale of our importing quantities
        //   floored may then sell less than the value exported, but we can just track that rn

        if (townExportsPrice < townImportsPrice)
        {
            for (int itemID = 0; itemID < 3; itemID++)
            {
                // if export move to player inventory
                if (Town.netProduction(itemID) < 0)
                {
                    // consumed, imported item
                    // add imported items from trader inventory scaled
                    int importQuantity = (int)(inventory[itemID] * townExportsPrice / (float)townImportsPrice);

                    Town.Wealth -= Town.appraise(itemID) * importQuantity;

                    Town.Stocks[itemID] += importQuantity;
                    inventory[itemID] -= importQuantity;
                }
                else
                {
                    // produced, exported item
                    // add exported items to trader inventory
                    int exportQuantity = (int)Town.Stocks[itemID];

                    Town.Wealth += Town.appraise(itemID) * exportQuantity;

                    inventory[itemID] += exportQuantity;
                    Town.Stocks[itemID] -= exportQuantity;
                }
            }
        }
        else if (townExportsPrice > townImportsPrice)
        {
            for (int itemID = 0; itemID < 3; itemID++)
            {
                // if export move to player inventory
                if (Town.netProduction(itemID) < 0)
                {
                    // consumed, imported item
                    // add imported items from trader inventory scaled
                    //int importQuantity = (int)(inventory[itemID] * townExportsPrice / (float)townImportsPrice);
                    int importQuantity = inventory[itemID];
                    
                    Town.Wealth -= Town.appraise(itemID) * importQuantity;

                    Town.Stocks[itemID] += importQuantity;
                    inventory[itemID] -= importQuantity;
                }
                else
                {
                    // produced, exported item
                    // add exported items to trader inventory
                    int exportQuantity = (int)(Town.Stocks[itemID] * townImportsPrice / (float)townExportsPrice);

                    Town.Wealth += Town.appraise(itemID) * exportQuantity;

                    inventory[itemID] += exportQuantity;
                    Town.Stocks[itemID] -= exportQuantity;
                }
            }
        }

    }
    void chooseDestination()
    {
        // get which item i have most of

        // find which town needs this item (has negative net prod)

        // town must be consuming an item we have some of

        // i don't want to go for the first valid item of the first valid town
        // shuffle ig

        Godot.Collections.Array<Town> TownOptions = Player.Instance.World.Towns.Duplicate();
        TownOptions.Remove(Town);
        TownOptions.Shuffle();

        Godot.Collections.Array<int> itemIDs = [0, 1, 2]; // idk man how else do randomise range
        itemIDs.Shuffle();
        

        foreach(int item in itemIDs)
        {
            if (inventory[item] == 0) continue; // skip to an item i have

            foreach (Town town in TownOptions)
            {
                if (town.netProduction(item) >= 0) continue; // skip to a town that wants the item

                journey.initJourney(Town, town);
                return;
            }
        }

        //GD.Print("there is no functioning town that consumes any item i have");
    }

    public void ShowInventory() => Player.Instance.UI.inventoryUI.Open(this); // called on hover
    public void HideInventory() => Player.Instance.UI.inventoryUI.Close(); // called on mouse exit
}
