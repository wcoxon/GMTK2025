using Godot;
using System;

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
    public int Population { get => data.population; set => data.population = value; }
    public int Wealth { get => data.wealth; set => data.wealth = value; }
    public float[] Stocks { get => data.stocks; set => data.stocks = value; }
    public float[] Production { get => data.production; } // per day
    public float[] Consumption { get => data.consumption; } // per day

    public override void _Ready()
    {
        // register for rumors.
        RumorView.towns.Add(data.townName, this);
        // data.price_multiplyers.CopyTo(data.prices, 0);
        for (int i = 0; i < 3; i++)
            data.prices[i] = PlayerView.instance.itemBaseValues[i] * data.price_multiplyers[i];
        data.base_consumption.CopyTo(data.consumption, 0);
        data.base_production.CopyTo(data.production, 0);
    }

    public void _on_area_3d_input_event(Node cam, InputEvent evt, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (evt is InputEventMouseButton mbEvent && Input.IsActionJustPressed("select"))
        {
            select();
        }
    }

    public void select() => PlayerView.instance.SelectedTown = this;

    public int appraise(Item item)
    {
        return (int)data.prices[(int)item];
    }

    const double UPDATE_INTERVAL = 8 * 3; // Update every 8 hours.
    private double next_update = UPDATE_INTERVAL; // Update
    public override void _PhysicsProcess(double delta)
    {
        next_update -= delta * PlayerView.instance.worldSpeed;
        if (next_update < 0)
        {
            updateStock();
            next_update += UPDATE_INTERVAL;
        }
    }

    static float price_by_demand(float base_price, float demand, float base_demand, float demand_elasticity)
    {
        return base_price * Mathf.Pow(demand / base_demand, 1 / demand_elasticity);
    }

    static float supply_by_price(float base_supply, float price, float base_price, float supply_elasticity)
    {
        return base_supply * Mathf.Pow(price / base_price, supply_elasticity);
    }

    static RandomNumberGenerator rng = new RandomNumberGenerator();
    void updateStock()
    {
        for (int item = 0; item < 3; item++)
        {
            data.production[item] = supply_by_price(data.base_production[item], data.prices[item], PlayerView.instance.itemBaseValues[item] * data.price_multiplyers[item], data.production_elasticity);
            data.consumption[item] = data.stock_selloff[item] * data.stocks[item] + data.supply_selloff[item] * data.production[item];
            data.prices[item] = price_by_demand(PlayerView.instance.itemBaseValues[item] * data.price_multiplyers[item], data.consumption[item], data.base_consumption[item], data.consumption_elasticity);
            data.stocks[item] += Mathf.Max(-data.stocks[item], (data.production[item] - rng.Randfn(data.consumption[item], data.consumption[item]/10))/3);
        }
    }    
}
