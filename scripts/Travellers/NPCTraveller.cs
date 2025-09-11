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


        int buyPrice = 0;

        for (int item = 0; item < 3; item++)
        {
            if (Town.netProduction(item) < 0)
            {
                // sell

                int sellAmount = inventory[item];

                inventory[item] -= sellAmount;
                Town.Stocks[item] += sellAmount;
                Money += Town.appraise(item) * sellAmount;
                
                GD.Print($"sold {sellAmount} {Game.itemNames[item]}");

            }
            else
            {
                // buy

                int buyAmount = (int)Town.Stocks[item];

                buyPrice += Town.appraise(item) * buyAmount;
            }
        }

        // now we will sell all our shit regardless here,

        // but we will now buy as much as we can too
        // by taking how much of the buy price we can afford with our money,
        // so if i have 1/2 of buy price, then i can buy half the items ig

        // i guess floor how much i buy though?

        float buyPortion = Mathf.Min(Money / (float)buyPrice,1);

        for (int item = 0; item < 3; item++)
        {
            if (Town.netProduction(item) < 0) continue;

            int buyAmount = (int)(buyPortion * Town.Stocks[item]);

            Town.Stocks[item] -= buyAmount;
            inventory[item] += buyAmount;
            Money -= buyAmount * Town.appraise(item);

            GD.Print($"bought {buyAmount} {Game.itemNames[item]}");

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

    public void ShowInventory()
    {
        // update inventory to hovered traveller
        UIController.Instance.inventoryUI.OpenInventory(this);


    } //=> Player.Instance.UI.inventoryUI.Open(this); // called on hover
    public void HideInventory()
    {
        // close inventory window
        UIController.Instance.inventoryUI.Close();


    } //=> Player.Instance.UI.inventoryUI.Close(); // called on mouse exit
}
