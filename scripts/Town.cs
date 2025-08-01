using Godot;
using System;
using System.Collections.Generic;

public partial class Town : Node3D
{
    // towns have production (item, rate)
    // consumption (item, rate) (could just use negative production)
    // population (scales production and consumption)
    // stock (list of (item, quantity)), price is calculated in getPrice(item) using quantity and consumption of the item


    //refs, replace with selection in _Ready later probab
    [Export] MeshInstance3D mesh;


    public bool Selected
    {
        set
        {
            mesh.SetInstanceShaderParameter("width", value ? 0.3 : 0);
        }
    }

    // scooping town data out of custom resource because then we can save town data onto resource files
    [Export] TownData data;

    public string TownName { get => data.townName; }
    public int Population { get => data.population; }
    public int Wealth { get => data.wealth;  }
    public int[] Stocks { get => data.stocks; }
    public int[] Production { get => data.production; }
    public int[] Consumption { get => data.consumption; }


    public void _on_area_3d_input_event(Node cam, InputEvent evt, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (evt is InputEventMouseButton mbEvent && Input.IsActionJustPressed("select"))
        {
            select();
        }
    }

    public void select()
    {
        PlayerView.instance.SelectedTown = this;
    }

    int appraise(Item item)
    {
        // price based on
        // how much of that item is in stock at this town

        // consumption, which itself is scaled by population

        // that's kind of it? well for now let's say that's it

        // so the consumption determines whether there's a shortage, since it defines the threshold of not enough and more than enough

        // a 'shortage' is when ,, the amount consumed in a day is more than how much they have? what if they produce the amount they consume per day, then they're fine
        // well then maybe the NET consumption per day gives them like 5 days until they run out that's a shortage

        int itemIndex = (int)item;
        int netConsumption = Consumption[itemIndex] - Production[itemIndex];

        // if net consumption is 0, there is no surplus or shortage of it, use some standard price ig

        float valueScale = 1;

        valueScale *= Consumption[itemIndex] + 1;
        valueScale /= Production[itemIndex] + 1;


        return (int)(valueScale * 5);
    }

    void updateStock(float deltaTime)
    {
        // produce or consume items over time based on deltaStock
    }

    
}
