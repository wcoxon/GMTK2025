using Godot;
using System.Collections.Generic;

public partial class Town : Node3D
{
    MeshInstance3D meshInstance;

    bool selected;
    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            meshInstance.SetInstanceShaderParameter("colour", new Color(1, 0, 0));
            meshInstance.SetInstanceShaderParameter("width", selected ? 0.3 : 0);
        }
    }

    TownData data;
    [Export] TownData Data { get => data; set => data = value; }

    public string TownName { get => data.townName; }
    public int Population { get => data.population; set => data.population = value; }
    public int Wealth { get => data.wealth; set => data.wealth = value; }
    public float[] Stocks { get => data.stocks; set => data.stocks = value; }
    public float[] Production { get => data.production; } // per day
    public float[] Consumption { get => data.consumption; } // per day
    public AudioStream Theme { get => data.theme; }

    public List<Traveller> currentTravellers;

    public override void _EnterTree()
    {
        meshInstance = GetNode<MeshInstance3D>("MeshInstance");

        meshInstance.Mesh = data.mesh;
        meshInstance.SetSurfaceOverrideMaterial(0, data.material);
    }

    public override void _Ready()
    {
        // register for rumors.
        RumorView.towns.Add(data.townName, this);

        for (int i = 0; i < 3; i++)
            data.prices[i] = PlayerView.Instance.itemBaseValues[i] * data.price_multiplyers[i];

        data.base_consumption.CopyTo(data.consumption, 0);
        data.base_production.CopyTo(data.production, 0);

        PlayerView.Instance.allTowns.Add(this);
    }

    public void _on_area_3d_input_event(Node cam, InputEvent e, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (e is InputEventMouseButton mbEvent && Input.IsActionJustPressed("select")) select();
    }
    public void hover()
    {
        if (Selected) return;
        meshInstance.SetInstanceShaderParameter("colour", new Color(1, 0.5f, 0.8f));
        meshInstance.SetInstanceShaderParameter("width", 0.1);
    }
    public void unhover()
    {
        if (Selected) return;
        meshInstance.SetInstanceShaderParameter("width", 0);
    }
    public void select() => PlayerView.Instance.SelectedTown = this;

    public int appraise(Item item) => (int)data.prices[(int)item];
    
    //ngl i don't like this at all sry
    const double UPDATE_INTERVAL = 8 * 3; // Update every 8 hours. assumes an hour is 3 seconds long
    private double next_update = UPDATE_INTERVAL; // Update
    public override void _PhysicsProcess(double delta)
    {
        next_update -= delta * PlayerView.Instance.worldSpeed;
        if (next_update < 0)
        {
            updateStock();
            next_update += UPDATE_INTERVAL;
        }
    }

    static float price_by_demand(float base_price, float demand, float base_demand, float demand_elasticity)
    => base_price * Mathf.Pow(demand / base_demand, 1 / demand_elasticity);
    

    static float supply_by_price(float base_supply, float price, float base_price, float supply_elasticity)
    => base_supply * Mathf.Pow(price / base_price, supply_elasticity);
    

    static RandomNumberGenerator rng = new RandomNumberGenerator();
    void updateStock()
    {
        for (int item = 0; item < 3; item++)
        {
            data.production[item] = supply_by_price(data.base_production[item], data.prices[item], PlayerView.Instance.itemBaseValues[item] * data.price_multiplyers[item], data.production_elasticity);
            data.consumption[item] = data.stock_selloff[item] * data.stocks[item] + data.supply_selloff[item] * data.production[item];
            data.prices[item] = price_by_demand(PlayerView.Instance.itemBaseValues[item] * data.price_multiplyers[item], data.consumption[item], data.base_consumption[item], data.consumption_elasticity);
            data.stocks[item] += Mathf.Max(-data.stocks[item], (data.production[item] - rng.Randfn(data.consumption[item], data.consumption[item] / 10)) / 3);
        }
    }
}
